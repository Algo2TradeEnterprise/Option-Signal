Imports System.Threading
Imports Utilities.Network
Imports System.Net.Http
Imports NLog

Public Class StockSelection
    Implements IDisposable

#Region "Logging and Status Progress"
    Public Shared Shadows logger As Logger = LogManager.GetCurrentClassLogger
#End Region

#Region "Events/Event handlers"
    Public Event DocumentDownloadComplete()
    Public Event DocumentRetryStatus(ByVal currentTry As Integer, ByVal totalTries As Integer)
    Public Event Heartbeat(ByVal msg As String)
    Public Event WaitingFor(ByVal elapsedSecs As Integer, ByVal totalSecs As Integer, ByVal msg As String)
    'The below functions are needed to allow the derived classes to raise the above two events
    Protected Overridable Sub OnDocumentDownloadComplete()
        RaiseEvent DocumentDownloadComplete()
    End Sub
    Protected Overridable Sub OnDocumentRetryStatus(ByVal currentTry As Integer, ByVal totalTries As Integer)
        RaiseEvent DocumentRetryStatus(currentTry, totalTries)
    End Sub
    Protected Overridable Sub OnHeartbeat(ByVal msg As String)
        RaiseEvent Heartbeat(msg)
    End Sub
    Protected Overridable Sub OnWaitingFor(ByVal elapsedSecs As Integer, ByVal totalSecs As Integer, ByVal msg As String)
        RaiseEvent WaitingFor(elapsedSecs, totalSecs, msg)
    End Sub
#End Region

    Private _cts As CancellationTokenSource
    Private ReadOnly _settings As SignalSettings
    Private ReadOnly ZerodhaEODHistoricalURL = "https://kitecharts-aws.zerodha.com/api/chart/{0}/day?api_key=kitefront&access_token=K&from={1}&to={2}"
    Private ReadOnly ZerodhaIntradayHistoricalURL = "https://kitecharts-aws.zerodha.com/api/chart/{0}/minute?api_key=kitefront&access_token=K&from={1}&to={2}"
    Private ReadOnly _tradingDay As Date = Date.MinValue

    Public Sub New(ByVal canceller As CancellationTokenSource, ByVal settings As SignalSettings, ByVal tradingDate As Date)
        _cts = canceller
        _settings = settings
        _tradingDay = tradingDate
    End Sub

    Private Async Function GetHistoricalCandleStickAsync(ByVal tradingSymbol As String, ByVal instrumentToken As String, ByVal fromDate As Date, ByVal toDate As Date, ByVal historicalDataType As TypeOfData) As Task(Of Dictionary(Of Date, Payload))
        Dim ret As Dictionary(Of Date, Payload) = Nothing
        Dim jsonDict As Dictionary(Of String, Object) = Nothing
        _cts.Token.ThrowIfCancellationRequested()
        Dim historicalDataURL As String = Nothing
        Select Case historicalDataType
            Case TypeOfData.Intraday
                historicalDataURL = String.Format(ZerodhaIntradayHistoricalURL, instrumentToken, fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"))
            Case TypeOfData.EOD
                historicalDataURL = String.Format(ZerodhaEODHistoricalURL, instrumentToken, fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"))
        End Select
        OnHeartbeat(String.Format("Fetching historical Data: {0}", historicalDataURL))
        Dim proxyToBeUsed As HttpProxy = Nothing
        Using browser As New HttpBrowser(proxyToBeUsed, Net.DecompressionMethods.GZip, New TimeSpan(0, 1, 0), _cts)
            AddHandler browser.DocumentDownloadComplete, AddressOf OnDocumentDownloadComplete
            AddHandler browser.Heartbeat, AddressOf OnHeartbeat
            AddHandler browser.WaitingFor, AddressOf OnWaitingFor
            AddHandler browser.DocumentRetryStatus, AddressOf OnDocumentRetryStatus
            'Get to the landing page first
            Dim l As Tuple(Of Uri, Object) = Await browser.NonPOSTRequestAsync(historicalDataURL,
                                                                                HttpMethod.Get,
                                                                                Nothing,
                                                                                True,
                                                                                Nothing,
                                                                                True,
                                                                                "application/json").ConfigureAwait(False)
            If l Is Nothing OrElse l.Item2 Is Nothing Then
                Throw New ApplicationException(String.Format("No response while getting historical data for: {0}", historicalDataURL))
            End If
            If l IsNot Nothing AndAlso l.Item2 IsNot Nothing Then
                jsonDict = l.Item2
            End If
            RemoveHandler browser.DocumentDownloadComplete, AddressOf OnDocumentDownloadComplete
            RemoveHandler browser.Heartbeat, AddressOf OnHeartbeat
            RemoveHandler browser.WaitingFor, AddressOf OnWaitingFor
            RemoveHandler browser.DocumentRetryStatus, AddressOf OnDocumentRetryStatus
        End Using
        If jsonDict IsNot Nothing AndAlso jsonDict.Count > 0 Then
            ret = Await GetChartFromHistoricalAsync(jsonDict, tradingSymbol).ConfigureAwait(False)
        End If
        Return ret
    End Function

    Public Async Function GetInstrumentData(ByVal allFutureInstruments As Dictionary(Of String, List(Of Date)), ByVal allCashInstruments As Dictionary(Of String, String), ByVal bannedStockList As List(Of String)) As Task(Of Dictionary(Of String, Decimal()))
        Dim ret As Dictionary(Of String, Decimal()) = Nothing
        If allFutureInstruments IsNot Nothing AndAlso allFutureInstruments.Count > 0 AndAlso allCashInstruments IsNot Nothing AndAlso allCashInstruments.Count > 0 Then
            Dim currentNFOInstruments As List(Of String) = Nothing
            For Each runningInstrument In allFutureInstruments
                If allCashInstruments.ContainsKey(runningInstrument.Key) AndAlso allCashInstruments(runningInstrument.Key).Trim <> "" AndAlso
                    (bannedStockList Is Nothing OrElse (bannedStockList IsNot Nothing AndAlso Not bannedStockList.Contains(runningInstrument.Key))) Then
                    If currentNFOInstruments Is Nothing Then currentNFOInstruments = New List(Of String)
                    currentNFOInstruments.Add(runningInstrument.Key)
                End If
            Next
            Dim lastTradingDay As Date = Date.MinValue
            Dim highATRStocks As Concurrent.ConcurrentDictionary(Of String, Decimal()) = Nothing
            Try
                If currentNFOInstruments IsNot Nothing AndAlso currentNFOInstruments.Count > 0 Then
                    For i As Integer = 0 To currentNFOInstruments.Count - 1 Step 20
                        Dim numberOfData As Integer = If(currentNFOInstruments.Count - i > 20, 20, currentNFOInstruments.Count - i)
                        Dim tasks As IEnumerable(Of Task(Of Boolean)) = Nothing
                        tasks = currentNFOInstruments.GetRange(i, numberOfData).Select(Async Function(y)
                                                                                           Try
                                                                                               _cts.Token.ThrowIfCancellationRequested()
                                                                                               Dim eodHistoricalData As Dictionary(Of Date, Payload) = Await GetHistoricalCandleStickAsync(y, allCashInstruments(y), _tradingDay.AddDays(-300), _tradingDay.AddDays(-1), TypeOfData.EOD).ConfigureAwait(False)
                                                                                               _cts.Token.ThrowIfCancellationRequested()
                                                                                               If eodHistoricalData IsNot Nothing AndAlso eodHistoricalData.Count > 100 Then
                                                                                                   Dim lastDayPayload As Payload = eodHistoricalData.LastOrDefault.Value
                                                                                                   If lastDayPayload.Close >= _settings.MinimumStockPrice AndAlso lastDayPayload.Close <= _settings.MaximumStockPrice Then
                                                                                                       _cts.Token.ThrowIfCancellationRequested()
                                                                                                       Dim ATRPayload As Dictionary(Of Date, Decimal) = Nothing
                                                                                                       CalculateATR(14, eodHistoricalData, ATRPayload)
                                                                                                       _cts.Token.ThrowIfCancellationRequested()
                                                                                                       Dim lastDayClosePrice As Decimal = eodHistoricalData.LastOrDefault.Value.Close
                                                                                                       lastTradingDay = eodHistoricalData.LastOrDefault.Key
                                                                                                       Dim atrPercentage As Decimal = (ATRPayload(eodHistoricalData.LastOrDefault.Key) / lastDayClosePrice) * 100
                                                                                                       If atrPercentage >= _settings.MinimumATRPercentage Then
                                                                                                           _cts.Token.ThrowIfCancellationRequested()
                                                                                                           If highATRStocks Is Nothing Then highATRStocks = New Concurrent.ConcurrentDictionary(Of String, Decimal())
                                                                                                           highATRStocks.TryAdd(y, {atrPercentage, lastDayClosePrice})
                                                                                                       End If
                                                                                                   End If
                                                                                               End If
                                                                                           Catch ex As Exception
                                                                                               logger.Error(ex)
                                                                                               Throw ex
                                                                                           End Try
                                                                                           Return True
                                                                                       End Function)

                        Dim mainTask As Task = Task.WhenAll(tasks)
                        Await mainTask.ConfigureAwait(False)
                        If mainTask.Exception IsNot Nothing Then
                            logger.Error(mainTask.Exception)
                            Throw mainTask.Exception
                        End If
                    Next
                End If
            Catch cex As TaskCanceledException
                logger.Error(cex)
                Throw cex
            Catch aex As AggregateException
                logger.Error(aex)
                Throw aex
            Catch ex As Exception
                logger.Error(ex)
                Throw ex
            End Try

            If highATRStocks IsNot Nothing AndAlso highATRStocks.Count > 0 Then
                Dim capableStocks As Dictionary(Of String, InstrumentDetails) = Nothing
                For Each stock In highATRStocks.OrderByDescending(Function(x)
                                                                      Return x.Value(0)
                                                                  End Function)
                    _cts.Token.ThrowIfCancellationRequested()
                    Dim intradayHistoricalData As Dictionary(Of Date, Payload) = Await GetHistoricalCandleStickAsync(stock.Key, allCashInstruments(stock.Key), lastTradingDay, lastTradingDay, TypeOfData.Intraday).ConfigureAwait(False)
                    If intradayHistoricalData IsNot Nothing AndAlso intradayHistoricalData.Count > 0 Then
                        Dim blankCandlePercentage As Decimal = CalculateBlankVolumePercentage(intradayHistoricalData)
                        Dim instrumentData As New InstrumentDetails With
                            {.TradingSymbol = stock.Key,
                             .ATR = stock.Value(0),
                             .Close = stock.Value(1),
                             .BlankCandlePercentage = blankCandlePercentage}
                        If capableStocks Is Nothing Then capableStocks = New Dictionary(Of String, InstrumentDetails)
                        capableStocks.Add(stock.Key, instrumentData)
                    End If
                Next
                If capableStocks IsNot Nothing AndAlso capableStocks.Count > 0 Then
                    Dim stocksLessThanMaxBlankCandlePercentage As IEnumerable(Of KeyValuePair(Of String, InstrumentDetails)) =
                                capableStocks.Where(Function(x)
                                                        Return x.Value.BlankCandlePercentage <> Decimal.MinValue AndAlso
                                                              x.Value.BlankCandlePercentage <= _settings.MinimumBlankCandlePercentage
                                                    End Function)
                    If stocksLessThanMaxBlankCandlePercentage IsNot Nothing AndAlso stocksLessThanMaxBlankCandlePercentage.Count > 0 Then
                        Dim stockCounter As Integer = 0
                        For Each stockData In stocksLessThanMaxBlankCandlePercentage.OrderByDescending(Function(x)
                                                                                                           Return x.Value.ATR
                                                                                                       End Function)
                            _cts.Token.ThrowIfCancellationRequested()
                            Dim slab As Decimal = CalculateSlab(stockData.Value.Close, stockData.Value.Close * stockData.Value.ATR)
                            If ret Is Nothing Then ret = New Dictionary(Of String, Decimal())
                            ret.Add(stockData.Key, {stockData.Value.ATR, slab})
                            stockCounter += 1
                            If stockCounter >= _settings.NumberOfStock Then Exit For
                        Next
                    End If
                End If
            End If
        End If
        Return ret
    End Function

    Private Async Function GetChartFromHistoricalAsync(ByVal historicalCandlesJSONDict As Dictionary(Of String, Object),
                                                       ByVal tradingSymbol As String) As Task(Of Dictionary(Of Date, Payload))
        Await Task.Delay(0, _cts.Token).ConfigureAwait(False)
        Dim ret As Dictionary(Of Date, Payload) = Nothing
        If historicalCandlesJSONDict.ContainsKey("data") Then
            Dim historicalCandlesDict As Dictionary(Of String, Object) = historicalCandlesJSONDict("data")
            If historicalCandlesDict.ContainsKey("candles") AndAlso historicalCandlesDict("candles").count > 0 Then
                Dim historicalCandles As ArrayList = historicalCandlesDict("candles")
                If ret Is Nothing Then ret = New Dictionary(Of Date, Payload)
                OnHeartbeat(String.Format("Generating Payload for {0}", tradingSymbol))
                Dim previousPayload As Payload = Nothing
                For Each historicalCandle In historicalCandles
                    _cts.Token.ThrowIfCancellationRequested()
                    Dim runningSnapshotTime As Date = Utilities.Time.GetDateTimeTillMinutes(historicalCandle(0))

                    Dim runningPayload As Payload = New Payload
                    With runningPayload
                        .PayloadDate = Utilities.Time.GetDateTimeTillMinutes(historicalCandle(0))
                        .TradingSymbol = tradingSymbol
                        .Open = historicalCandle(1)
                        .High = historicalCandle(2)
                        .Low = historicalCandle(3)
                        .Close = historicalCandle(4)
                        .Volume = historicalCandle(5)
                        .PreviousPayload = previousPayload
                    End With
                    previousPayload = runningPayload
                    ret.Add(runningSnapshotTime, runningPayload)
                Next
            End If
        End If
        Return ret
    End Function

    Private Sub CalculateATR(ByVal ATRPeriod As Integer, ByVal inputPayload As Dictionary(Of Date, Payload), ByRef outputPayload As Dictionary(Of Date, Decimal))
        'Using WILDER Formula
        If inputPayload IsNot Nothing AndAlso inputPayload.Count > 0 Then
            OnHeartbeat(String.Format("Calculating ATR for {0}", inputPayload.LastOrDefault.Value.TradingSymbol))
            If inputPayload.Count < 100 Then
                Throw New ApplicationException("Can't Calculate ATR")
            End If
            Dim firstPayload As Boolean = True
            Dim highLow As Double = Nothing
            Dim highClose As Double = Nothing
            Dim lowClose As Double = Nothing
            Dim TR As Double = Nothing
            Dim SumTR As Double = 0.00
            Dim AvgTR As Double = 0.00
            Dim counter As Integer = 0
            outputPayload = New Dictionary(Of Date, Decimal)
            For Each runningInputPayload In inputPayload
                counter += 1
                highLow = runningInputPayload.Value.High - runningInputPayload.Value.Low
                If firstPayload = True Then
                    TR = highLow
                    firstPayload = False
                Else
                    highClose = Math.Abs(runningInputPayload.Value.High - runningInputPayload.Value.PreviousPayload.Close)
                    lowClose = Math.Abs(runningInputPayload.Value.Low - runningInputPayload.Value.PreviousPayload.Close)
                    TR = Math.Max(highLow, Math.Max(highClose, lowClose))
                End If
                SumTR = SumTR + TR
                If counter = ATRPeriod Then
                    AvgTR = SumTR / ATRPeriod
                    outputPayload.Add(runningInputPayload.Value.PayloadDate, AvgTR)
                ElseIf counter > ATRPeriod Then
                    AvgTR = (outputPayload(runningInputPayload.Value.PreviousPayload.PayloadDate) * (ATRPeriod - 1) + TR) / ATRPeriod
                    outputPayload.Add(runningInputPayload.Value.PayloadDate, AvgTR)
                Else
                    AvgTR = SumTR / counter
                    outputPayload.Add(runningInputPayload.Value.PayloadDate, AvgTR)
                End If
            Next
        End If
    End Sub

    Private Function CalculateBlankVolumePercentage(ByVal inputPayload As Dictionary(Of Date, Payload)) As Decimal
        Dim ret As Decimal = Decimal.MinValue
        If inputPayload IsNot Nothing AndAlso inputPayload.Count > 0 Then
            Dim blankCandlePayload As IEnumerable(Of KeyValuePair(Of Date, Payload)) = inputPayload.Where(Function(x)
                                                                                                              Return x.Value.Open = x.Value.Low AndAlso
                                                                                                                  x.Value.Low = x.Value.High AndAlso
                                                                                                                  x.Value.High = x.Value.Close
                                                                                                          End Function)
            If blankCandlePayload IsNot Nothing AndAlso blankCandlePayload.Count > 0 Then
                ret = Math.Round((blankCandlePayload.Count / inputPayload.Count) * 100, 2)
            Else
                ret = 0
            End If
        End If
        Return ret
    End Function

    Private Function CalculateSlab(ByVal price As Decimal, ByVal atr As Decimal) As Decimal
        Dim ret As Decimal = 0.25
        Dim slabList As List(Of Decimal) = New List(Of Decimal) From {0.25, 0.5, 1, 2.5, 5, 10, 25}
        Dim supportedSlabList As List(Of Decimal) = slabList.FindAll(Function(x)
                                                                         Return x <= atr / 8
                                                                     End Function)
        If supportedSlabList IsNot Nothing AndAlso supportedSlabList.Count > 0 Then
            ret = supportedSlabList.Max
            If price * 1 / 100 < ret Then
                Dim newSupportedSlabList As List(Of Decimal) = supportedSlabList.FindAll(Function(x)
                                                                                             Return x <= price * 1 / 100
                                                                                         End Function)
                If newSupportedSlabList IsNot Nothing AndAlso newSupportedSlabList.Count > 0 Then
                    ret = newSupportedSlabList.Max
                End If
            End If
        End If
        Return ret
    End Function

    Private Class InstrumentDetails
        Public TradingSymbol As String
        Public ATR As Decimal
        Public Close As Decimal
        Public BlankCandlePercentage As Decimal
    End Class

    Enum TypeOfData
        Intraday = 1
        EOD
    End Enum

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class