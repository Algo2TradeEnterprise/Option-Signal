Imports NLog
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

Public Class InstrumentDetails
    Implements INotifyPropertyChanged

#Region "Logging and Status Progress"
    Public Shared logger As Logger = LogManager.GetCurrentClassLogger
#End Region

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Sub NotifyPropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Private ReadOnly _settings As SignalSettings
    Public Sub New(ByVal settings As SignalSettings)
        _settings = settings
    End Sub

    <Display(Name:="Instrument Name", Order:=0, AutoGenerateField:=True)>
    Public Property OriginatingInstrument As String

    Private _LastUpdateTime As Date
    <Display(Name:="Last Update Time", Order:=1, AutoGenerateField:=True)>
    Public Property LastUpdateTime As Date
        Get
            Return _LastUpdateTime
        End Get
        Set(value As Date)
            If _LastUpdateTime <> value Then
                _LastUpdateTime = value

                'logger.Fatal(Me.ToString)
            End If
        End Set
    End Property

    <Display(Name:="ATR %", Order:=2, AutoGenerateField:=True)>
    Public Property ATR As Decimal

    <Display(Name:="Slab", Order:=3, AutoGenerateField:=True)>
    Public Property Slab As Decimal

    <Display(Name:="OI PCR", Order:=4, AutoGenerateField:=True)>
    Public ReadOnly Property OIPCR As Decimal
        Get
            If SumOfCallsOI <> Long.MinValue AndAlso SumOfPutsOI <> Long.MinValue AndAlso SumOfCallsOI <> 0 Then
                Return Math.Round(SumOfPutsOI / SumOfCallsOI, 2)
            Else
                Return 0
            End If
        End Get
    End Property

    <Display(Name:="Volume PCR", Order:=5, AutoGenerateField:=True)>
    Public ReadOnly Property VolumePCR As Decimal
        Get
            If SumOfCallsVolume <> Long.MinValue AndAlso SumOfPutsVolume <> Long.MinValue AndAlso SumOfCallsVolume <> 0 Then
                Return Math.Round(SumOfPutsVolume / SumOfCallsVolume, 2)
            Else
                Return 0
            End If
        End Get
    End Property

    <Display(Name:="Sentiment", Order:=6, AutoGenerateField:=True)>
    Public ReadOnly Property Sentiment As String
        Get
            If Me.OIPCR <> Decimal.MinValue AndAlso Me.VolumePCR <> Decimal.MinValue AndAlso
                Me.OIPCR <> 0 AndAlso Me.VolumePCR <> 0 Then
                If Me.OIPCR <= _settings.RatioLowerValue AndAlso Me.VolumePCR <= _settings.RatioLowerValue Then
                    If _firstBullish = Date.MinValue Then
                        _firstBullish = Now
                        NotifyPropertyChanged("FirstBullish")
                    End If
                    _lastBullish = Now
                    NotifyPropertyChanged("LastBullish")
                    Return Color.Green.Name
                ElseIf Me.OIPCR >= _settings.RatioUpperValue AndAlso Me.VolumePCR >= _settings.RatioUpperValue Then
                    If _firstBearish = Date.MinValue Then
                        _firstBearish = Now
                        NotifyPropertyChanged("FirstBearish")
                    End If
                    _lastBearish = Now
                    NotifyPropertyChanged("LastBearish")
                    Return Color.Red.Name
                Else
                    Return Color.White.Name
                End If
            Else
                Return Color.White.Name
            End If
        End Get
    End Property

    Private _firstBullish As Date = Date.MinValue
    <Display(Name:="First Bullish", Order:=7, AutoGenerateField:=True)>
    Public ReadOnly Property FirstBullish As Date
        Get
            Return _firstBullish
        End Get
    End Property

    Private _firstBearish As Date = Date.MinValue
    <Display(Name:="First Bearish", Order:=8, AutoGenerateField:=True)>
    Public ReadOnly Property FirstBearish As Date
        Get
            Return _firstBearish
        End Get
    End Property

    Private _lastBullish As Date = Date.MinValue
    <Display(Name:="Last Bullish", Order:=9, AutoGenerateField:=True)>
    Public ReadOnly Property LastBullish As Date
        Get
            Return _lastBullish
        End Get
    End Property

    Private _lastBearish As Date = Date.MinValue
    <Display(Name:="Last Bearish", Order:=10, AutoGenerateField:=True)>
    Public ReadOnly Property LastBearish As Date
        Get
            Return _lastBearish
        End Get
    End Property


    <Display(Name:="", Order:=11, AutoGenerateField:=True)>
    Public Property Seperator1 As String

    <Display(Name:="Sum of Puts OI", Order:=12, AutoGenerateField:=True)>
    Public Property SumOfPutsOI As Long

    <Display(Name:="Sum of Calls OI", Order:=13, AutoGenerateField:=True)>
    Public Property SumOfCallsOI As Long

    <Display(Name:="Sum of Puts Volume", Order:=14, AutoGenerateField:=True)>
    Public Property SumOfPutsVolume As Long

    <Display(Name:="Sum of Calls Volume", Order:=15, AutoGenerateField:=True)>
    Public Property SumOfCallsVolume As Long


#Region "Non Relevant"
    <Display(Name:="", Order:=50, AutoGenerateField:=True)>
    Public Property Seperator50 As String

    <Display(Name:="Open", Order:=51, AutoGenerateField:=True)>
    Public Property Open As Decimal

    <Display(Name:="Low", Order:=52, AutoGenerateField:=True)>
    Public Property Low As Decimal

    <Display(Name:="High", Order:=53, AutoGenerateField:=True)>
    Public Property High As Decimal

    <Display(Name:="Close", Order:=54, AutoGenerateField:=True)>
    Public Property Close As Decimal

    <Display(Name:="Volume", Order:=55, AutoGenerateField:=True)>
    Public Property Volume As Decimal

    <Display(Name:="LTP", Order:=56, AutoGenerateField:=True)>
    Public Property LTP As Decimal

    <Display(Name:="Change %", Order:=57, AutoGenerateField:=True)>
    Public ReadOnly Property ChangePer As Decimal
        Get
            If LTP <> Decimal.MinValue AndAlso PreviousClose <> Decimal.MinValue AndAlso LTP <> 0 Then
                Return Math.Round(((PreviousClose / LTP) - 1) * 100, 2)
            Else
                Return 0
            End If
        End Get
    End Property
#End Region

#Region "Non Browsable"
    <Display(Name:="Previous Close", Order:=100, AutoGenerateField:=False)>
    Public Property PreviousClose As Decimal

    <Display(Name:="Previous Day", Order:=100, AutoGenerateField:=False)>
    Public Property PreviousDay As Date

    <Display(Name:="Cash Trading Symbol", Order:=100, AutoGenerateField:=False)>
    Public Property CashTradingSymbol As String

    <Display(Name:="Instrument Token", Order:=100, AutoGenerateField:=False)>
    Public Property CashInstrumentToken As String

    <System.ComponentModel.Browsable(False)>
    Public Property OptionInstruments As Dictionary(Of String, OptionInstrumentDetails)
    <System.ComponentModel.Browsable(False)>
    Public Property PEInstrumentsPayloads As Concurrent.ConcurrentDictionary(Of String, Payload)
    <System.ComponentModel.Browsable(False)>
    Public Property CEInstrumentsPayloads As Concurrent.ConcurrentDictionary(Of String, Payload)
#End Region

    'Public Overrides Function ToString() As String
    '    Return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24}",
    '                         Me.OriginatingInstrument, Me.LastUpdateTime.ToString("HH:mm:ss"), Me.LTP, Me.ChangePer, Me.ATR, Me.Slab,
    '                         Me.SumOfPutsOI, Me.SumOfCallsOI, Me.OICPC, Me.OIPCC, Me.OICTR, Me.OIPTR, "",
    '                         Me.SumOfPutsOIChange, Me.SumOfCallsOIChange, Me.OIChangeCPC, Me.OIChangePCC, Me.OIChangeCTR, Me.OIChangePTR, "",
    '                         Me.Open, Me.Low, Me.High, Me.Close, Me.Volume)
    'End Function

End Class
