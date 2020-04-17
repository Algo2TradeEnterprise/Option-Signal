Imports System.Threading
Imports Utilities.Network
Imports System.Net
Imports System.Net.Http

Public Class DataFetcher

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

#Region "Enum"
    Enum DataType
        EOD = 1
        Intraday
    End Enum
#End Region

    Private ReadOnly _cts As CancellationTokenSource
    Private ReadOnly _instrument As InstrumentDetails
    Private ReadOnly _fetchDataWithAPI As Boolean
    Private ReadOnly _fetchDataWithoutAPI As Boolean
    Private ReadOnly _tradingDate As Date
    Private ReadOnly _settings As SignalSettings

    Public Sub New(ByVal canceller As CancellationTokenSource,
                   ByVal processInstrument As InstrumentDetails,
                   ByVal fetchDataWithAPI As Boolean,
                   ByVal fetchDataWithoutAPI As Boolean,
                   ByVal tradingDate As Date,
                   ByVal settings As SignalSettings)
        _cts = canceller
        _instrument = processInstrument
        _fetchDataWithAPI = fetchDataWithAPI
        _fetchDataWithoutAPI = fetchDataWithoutAPI
        _tradingDate = tradingDate
        _settings = settings
    End Sub

    Public Async Function StartFetchingAsync() As Task
        Try
            _cts.Token.ThrowIfCancellationRequested()
            Dim eodData As Dictionary(Of Date, Payload) = Await GetHistoricalDataAsync(_instrument.CashInstrumentToken, _instrument.CashTradingSymbol, _tradingDate.Date.AddDays(-150), _tradingDate.Date, DataType.EOD, Nothing, _fetchDataWithAPI, _fetchDataWithoutAPI, _cts)
            _cts.Token.ThrowIfCancellationRequested()
            If eodData IsNot Nothing AndAlso eodData.Count > 0 Then
                If _instrument.Open = Decimal.MinValue OrElse _instrument.Open <> eodData.LastOrDefault.Value.Open Then
                    _instrument.Open = eodData.LastOrDefault.Value.Open
                    _instrument.NotifyPropertyChanged("Open")
                End If
                If _instrument.Low = Decimal.MinValue OrElse _instrument.Low <> eodData.LastOrDefault.Value.Low Then
                    _instrument.Low = eodData.LastOrDefault.Value.Low
                    _instrument.NotifyPropertyChanged("Low")
                End If
                If _instrument.High = Decimal.MinValue OrElse _instrument.High <> eodData.LastOrDefault.Value.High Then
                    _instrument.High = eodData.LastOrDefault.Value.High
                    _instrument.NotifyPropertyChanged("High")
                End If
                If _instrument.Close = Decimal.MinValue OrElse _instrument.Close <> eodData.LastOrDefault.Value.Close Then
                    _instrument.Close = eodData.LastOrDefault.Value.Close
                    _instrument.NotifyPropertyChanged("Close")
                End If
                If _instrument.Volume = Decimal.MinValue OrElse _instrument.Volume <> eodData.LastOrDefault.Value.Volume Then
                    _instrument.Volume = eodData.LastOrDefault.Value.Volume
                    _instrument.NotifyPropertyChanged("Volume")
                End If
                If _instrument.LTP = Decimal.MinValue OrElse _instrument.LTP <> eodData.LastOrDefault.Value.Close Then
                    _instrument.LTP = eodData.LastOrDefault.Value.Close
                    _instrument.NotifyPropertyChanged("LTP")
                    _instrument.NotifyPropertyChanged("ChangePer")
                End If
                If (_instrument.PreviousDay = Date.MinValue OrElse _instrument.PreviousDay.Date = Now.Date) AndAlso
                    eodData.LastOrDefault.Value.PreviousPayload IsNot Nothing Then
                    If eodData.LastOrDefault.Value.PayloadDate < _tradingDate Then
                        _instrument.PreviousDay = eodData.LastOrDefault.Value.PayloadDate.Date
                        _instrument.PreviousClose = eodData.LastOrDefault.Value.Close
                        _instrument.NotifyPropertyChanged("PreviousClose")
                    Else
                        _instrument.PreviousDay = eodData.LastOrDefault.Value.PreviousPayload.PayloadDate
                        _instrument.PreviousClose = eodData.LastOrDefault.Value.PreviousPayload.Close
                        _instrument.NotifyPropertyChanged("PreviousClose")
                    End If

                    'Dim atrPayload As Dictionary(Of Date, Decimal) = Nothing
                    'CalculateATR(14, eodData, atrPayload)
                    'If atrPayload IsNot Nothing AndAlso atrPayload.ContainsKey(_instrument.PreviousDay) Then
                    '    Dim atr As Decimal = atrPayload(_instrument.PreviousDay)

                    '    _instrument.ATR = Math.Round((atr / _instrument.PreviousClose) * 100, 2)
                    '    _instrument.NotifyPropertyChanged("ATR")

                    '    _instrument.Slab = CalculateSlab(_instrument.PreviousClose, atr)
                    '    _instrument.NotifyPropertyChanged("Slab")
                    'End If
                End If



                If _instrument.OptionInstruments IsNot Nothing AndAlso _instrument.OptionInstruments.Count > 0 Then
                    Try
                        Dim numberOfParallelHit As Integer = _settings.OptionStockParallelHit
                        For i As Integer = 0 To _instrument.OptionInstruments.Count - 1 Step numberOfParallelHit
                            _cts.Token.ThrowIfCancellationRequested()
                            Dim numberOfData As Integer = If(_instrument.OptionInstruments.Count - i > numberOfParallelHit, numberOfParallelHit, _instrument.OptionInstruments.Count - i)
                            Dim tasks As IEnumerable(Of Task(Of Boolean)) = Nothing
                            tasks = _instrument.OptionInstruments.Values.ToList.GetRange(i, numberOfData).Select(Async Function(x)
                                                                                                                     Try
                                                                                                                         Await GetOptionDataAsync(_instrument, x, _tradingDate).ConfigureAwait(False)
                                                                                                                     Catch ex As Exception
                                                                                                                         Throw ex
                                                                                                                     End Try
                                                                                                                     Return True
                                                                                                                 End Function)

                            Dim mainTask As Task = Task.WhenAll(tasks)
                            Await mainTask.ConfigureAwait(False)
                            If mainTask.Exception IsNot Nothing Then
                                Throw mainTask.Exception
                            End If
                        Next
                    Catch cex As TaskCanceledException
                        Throw cex
                    Catch aex As AggregateException
                        Throw aex
                    Catch ex As Exception
                        Throw ex
                    End Try
                End If

                If _instrument.PEInstrumentsPayloads IsNot Nothing AndAlso _instrument.PEInstrumentsPayloads.Count > 0 Then
                    Dim sumOfPutOI As Decimal = _instrument.PEInstrumentsPayloads.Sum(Function(x)
                                                                                          Return x.Value.OI
                                                                                      End Function)

                    If _instrument.SumOfPutsOI = Long.MinValue OrElse _instrument.SumOfPutsOI <> sumOfPutOI Then
                        _instrument.SumOfPutsOI = sumOfPutOI
                        _instrument.NotifyPropertyChanged("SumOfPutsOI")
                        _instrument.NotifyPropertyChanged("OIPCR")
                        _instrument.NotifyPropertyChanged("Sentiment")
                    End If

                    Dim sumOfPutVolume As Decimal = _instrument.PEInstrumentsPayloads.Sum(Function(x)
                                                                                              Return x.Value.Volume
                                                                                          End Function)

                    If _instrument.SumOfPutsVolume = Long.MinValue OrElse _instrument.SumOfPutsVolume <> sumOfPutVolume Then
                        _instrument.SumOfPutsVolume = sumOfPutVolume
                        _instrument.NotifyPropertyChanged("SumOfPutsVolume")
                        _instrument.NotifyPropertyChanged("VolumePCR")
                        _instrument.NotifyPropertyChanged("Sentiment")
                    End If
                End If

                If _instrument.CEInstrumentsPayloads IsNot Nothing AndAlso _instrument.CEInstrumentsPayloads.Count > 0 Then
                    Dim sumOfCallOI As Decimal = _instrument.CEInstrumentsPayloads.Sum(Function(x)
                                                                                           Return x.Value.OI
                                                                                       End Function)

                    If _instrument.SumOfCallsOI = Long.MinValue OrElse _instrument.SumOfCallsOI <> sumOfCallOI Then
                        _instrument.SumOfCallsOI = sumOfCallOI
                        _instrument.NotifyPropertyChanged("SumOfCallsOI")
                        _instrument.NotifyPropertyChanged("OIPCR")
                        _instrument.NotifyPropertyChanged("Sentiment")
                    End If

                    Dim sumOfCallVolume As Decimal = _instrument.CEInstrumentsPayloads.Sum(Function(x)
                                                                                               Return x.Value.Volume
                                                                                           End Function)

                    If _instrument.SumOfCallsVolume = Long.MinValue OrElse _instrument.SumOfCallsVolume <> sumOfCallVolume Then
                        _instrument.SumOfCallsVolume = sumOfCallVolume
                        _instrument.NotifyPropertyChanged("SumOfCallsVolume")
                        _instrument.NotifyPropertyChanged("VolumePCR")
                        _instrument.NotifyPropertyChanged("Sentiment")
                    End If
                End If

                _instrument.LastUpdateTime = Now
                _instrument.NotifyPropertyChanged("LastUpdateTime")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Async Function GetOptionDataAsync(ByVal originatingInstrument As InstrumentDetails, ByVal optionInstrument As OptionInstrumentDetails, ByVal tradingDate As Date) As Task
        Dim optionEodData As Dictionary(Of Date, Payload) = Await GetHistoricalDataAsync(optionInstrument.InstrumentToken, optionInstrument.TradingSymbol, tradingDate.Date.AddDays(-15), tradingDate.Date, DataType.EOD, Nothing, _fetchDataWithAPI, _fetchDataWithoutAPI, _cts)
        _cts.Token.ThrowIfCancellationRequested()
        If optionEodData IsNot Nothing AndAlso optionEodData.Count > 0 Then
            If optionInstrument.InstrumentType = "PE" Then
                If originatingInstrument.PEInstrumentsPayloads Is Nothing Then originatingInstrument.PEInstrumentsPayloads = New Concurrent.ConcurrentDictionary(Of String, Payload)
                'originatingInstrument.PEInstrumentsPayloads.AddOrUpdate(optionInstrument.StrikePrice, optionEodData.Values.LastOrDefault, Function(key, value) optionEodData.Values.LastOrDefault)
                originatingInstrument.PEInstrumentsPayloads.AddOrUpdate(optionInstrument.TradingSymbol, optionEodData.Values.LastOrDefault, Function(key, value) optionEodData.Values.LastOrDefault)
            ElseIf optionInstrument.InstrumentType = "CE" Then
                If originatingInstrument.CEInstrumentsPayloads Is Nothing Then originatingInstrument.CEInstrumentsPayloads = New Concurrent.ConcurrentDictionary(Of String, Payload)
                'originatingInstrument.CEInstrumentsPayloads.AddOrUpdate(optionInstrument.StrikePrice, optionEodData.Values.LastOrDefault, Function(key, value) optionEodData.Values.LastOrDefault)
                originatingInstrument.CEInstrumentsPayloads.AddOrUpdate(optionInstrument.TradingSymbol, optionEodData.Values.LastOrDefault, Function(key, value) optionEodData.Values.LastOrDefault)
            End If
        End If
    End Function

    Public Shared Async Function GetHistoricalDataAsync(ByVal instrumentToken As String, ByVal tradingSymbol As String, ByVal startDate As Date, ByVal endDate As Date, ByVal typeOfData As DataType,
                                                        ByVal zerodhaDetails As ZerodhaLogin, ByVal fetchDataWithAPI As Boolean, ByVal fetchDataWithoutAPI As Boolean, ByVal canceller As CancellationTokenSource) As Task(Of Dictionary(Of Date, Payload))
        Dim ret As Dictionary(Of Date, Payload) = Nothing
        Dim AWSZerodhaEODHistoricalURL As String = "https://kitecharts-aws.zerodha.com/api/chart/{0}/day?oi=1&api_key=kitefront&access_token=K&from={1}&to={2}"
        Dim AWSZerodhaIntradayHistoricalURL As String = "https://kitecharts-aws.zerodha.com/api/chart/{0}/minute?oi=1&api_key=kitefront&access_token=K&from={1}&to={2}"
        Dim ZerodhaEODHistoricalURL As String = "https://kite.zerodha.com/oms/instruments/historical/{0}/day?&oi=1&from={1}&to={2}"
        Dim ZerodhaIntradayHistoricalURL As String = "https://kite.zerodha.com/oms/instruments/historical/{0}/minute?oi=1&from={1}&to={2}"
        Dim ZerodhaHistoricalURL As String = Nothing
        Select Case typeOfData
            Case DataType.EOD
                If fetchDataWithAPI Then
                    ZerodhaHistoricalURL = ZerodhaEODHistoricalURL
                ElseIf fetchDataWithoutAPI Then
                    ZerodhaHistoricalURL = AWSZerodhaEODHistoricalURL
                End If
            Case DataType.Intraday
                If fetchDataWithAPI Then
                    ZerodhaHistoricalURL = ZerodhaIntradayHistoricalURL
                ElseIf fetchDataWithoutAPI Then
                    ZerodhaHistoricalURL = AWSZerodhaIntradayHistoricalURL
                End If
        End Select
        If ZerodhaHistoricalURL IsNot Nothing AndAlso instrumentToken IsNot Nothing AndAlso instrumentToken <> "" Then
            Dim historicalDataURL As String = String.Format(ZerodhaHistoricalURL, instrumentToken, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"))
            'OnHeartbeat(String.Format("Fetching historical Data: {0}", historicalDataURL))
            Dim historicalCandlesJSONDict As Dictionary(Of String, Object) = Nothing

            ServicePointManager.Expect100Continue = False
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            ServicePointManager.ServerCertificateValidationCallback = Function(s, Ca, CaC, sslPE)
                                                                          Return True
                                                                      End Function

            If fetchDataWithAPI Then
                Dim proxyToBeUsed As HttpProxy = Nothing
                Using browser As New HttpBrowser(proxyToBeUsed, Net.DecompressionMethods.GZip Or DecompressionMethods.Deflate Or DecompressionMethods.None, New TimeSpan(0, 1, 0), canceller)
                    'AddHandler browser.DocumentDownloadComplete, AddressOf OnDocumentDownloadComplete
                    'AddHandler browser.Heartbeat, AddressOf OnHeartbeat
                    'AddHandler browser.WaitingFor, AddressOf OnWaitingFor
                    'AddHandler browser.DocumentRetryStatus, AddressOf OnDocumentRetryStatus

                    Dim headers As New Dictionary(Of String, String)
                    headers.Add("Host", "kite.zerodha.com")
                    headers.Add("Accept", "*/*")
                    headers.Add("Accept-Encoding", "gzip, deflate")
                    headers.Add("Accept-Language", "en-US,en;q=0.9,hi;q=0.8,ko;q=0.7")
                    headers.Add("Authorization", String.Format("enctoken {0}", zerodhaDetails.ENCToken))
                    headers.Add("Referer", "https://kite.zerodha.com/static/build/chart.html?v=2.4.0")
                    headers.Add("sec-fetch-mode", "cors")
                    headers.Add("sec-fetch-site", "same-origin")
                    headers.Add("Connection", "keep-alive")

                    canceller.Token.ThrowIfCancellationRequested()
                    Dim l As Tuple(Of Uri, Object) = Await browser.NonPOSTRequestAsync(historicalDataURL,
                                                                                            HttpMethod.Get,
                                                                                            Nothing,
                                                                                            False,
                                                                                            headers,
                                                                                            True,
                                                                                            "application/json").ConfigureAwait(False)
                    canceller.Token.ThrowIfCancellationRequested()
                    If l IsNot Nothing AndAlso l.Item2 IsNot Nothing Then
                        historicalCandlesJSONDict = l.Item2
                    End If

                    'RemoveHandler browser.DocumentDownloadComplete, AddressOf OnDocumentDownloadComplete
                    'RemoveHandler browser.Heartbeat, AddressOf OnHeartbeat
                    'RemoveHandler browser.WaitingFor, AddressOf OnWaitingFor
                    'RemoveHandler browser.DocumentRetryStatus, AddressOf OnDocumentRetryStatus
                End Using
            ElseIf fetchDataWithoutAPI Then
                Dim proxyToBeUsed As HttpProxy = Nothing
                Using browser As New HttpBrowser(proxyToBeUsed, Net.DecompressionMethods.GZip, New TimeSpan(0, 1, 0), canceller)
                    'AddHandler browser.DocumentDownloadComplete, AddressOf OnDocumentDownloadComplete
                    'AddHandler browser.Heartbeat, AddressOf OnHeartbeat
                    'AddHandler browser.WaitingFor, AddressOf OnWaitingFor
                    'AddHandler browser.DocumentRetryStatus, AddressOf OnDocumentRetryStatus

                    canceller.Token.ThrowIfCancellationRequested()
                    Dim l As Tuple(Of Uri, Object) = Await browser.NonPOSTRequestAsync(historicalDataURL,
                                                                                        HttpMethod.Get,
                                                                                        Nothing,
                                                                                        True,
                                                                                        Nothing,
                                                                                        True,
                                                                                        "application/json").ConfigureAwait(False)
                    canceller.Token.ThrowIfCancellationRequested()
                    If l IsNot Nothing AndAlso l.Item2 IsNot Nothing Then
                        historicalCandlesJSONDict = l.Item2
                    End If

                    'RemoveHandler browser.DocumentDownloadComplete, AddressOf OnDocumentDownloadComplete
                    'RemoveHandler browser.Heartbeat, AddressOf OnHeartbeat
                    'RemoveHandler browser.WaitingFor, AddressOf OnWaitingFor
                    'RemoveHandler browser.DocumentRetryStatus, AddressOf OnDocumentRetryStatus
                End Using
            End If

            If historicalCandlesJSONDict IsNot Nothing AndAlso historicalCandlesJSONDict.Count > 0 AndAlso
                historicalCandlesJSONDict.ContainsKey("data") Then
                Dim historicalCandlesDict As Dictionary(Of String, Object) = historicalCandlesJSONDict("data")
                If historicalCandlesDict.ContainsKey("candles") AndAlso historicalCandlesDict("candles").count > 0 Then
                    Dim historicalCandles As ArrayList = historicalCandlesDict("candles")
                    'OnHeartbeat(String.Format("Generating Payload for {0}", tradingSymbol))
                    Dim previousPayload As Payload = Nothing
                    For Each historicalCandle As ArrayList In historicalCandles
                        canceller.Token.ThrowIfCancellationRequested()
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
                            .OI = historicalCandle(6)
                            .PreviousPayload = previousPayload
                        End With
                        If ret Is Nothing Then ret = New Dictionary(Of Date, Payload)
                        ret.Add(runningSnapshotTime, runningPayload)
                        previousPayload = runningPayload
                    Next
                End If
            End If
        End If
        Return ret
    End Function

    Private Sub CalculateATR(ByVal ATRPeriod As Integer, ByVal inputPayload As Dictionary(Of Date, Payload), ByRef outputPayload As Dictionary(Of Date, Decimal))
        If inputPayload IsNot Nothing AndAlso inputPayload.Count > 0 Then
            Dim firstPayload As Boolean = True
            Dim HighLow As Double = Nothing
            Dim HighClose As Double = Nothing
            Dim LowClose As Double = Nothing
            Dim PreviousClose As Double = Nothing
            Dim TR As Double = Nothing
            Dim SumTR As Double = 0.00
            Dim AvgTR As Double = 0.00
            Dim counter As Integer = 0
            outputPayload = New Dictionary(Of Date, Decimal)
            For Each runningInputPayload In inputPayload
                counter += 1
                HighLow = runningInputPayload.Value.High - runningInputPayload.Value.Low
                If firstPayload = True Then
                    TR = HighLow
                    firstPayload = False
                Else
                    HighClose = Math.Abs(runningInputPayload.Value.High - runningInputPayload.Value.PreviousPayload.Close)
                    LowClose = Math.Abs(runningInputPayload.Value.Low - runningInputPayload.Value.PreviousPayload.Close)
                    TR = Math.Max(HighLow, Math.Max(HighClose, LowClose))
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
End Class
