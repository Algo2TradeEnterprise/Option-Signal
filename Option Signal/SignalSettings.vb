Imports System.IO

<Serializable>
Public Class SignalSettings
    Public Shared Property SettingsFilename As String = Path.Combine(My.Application.Info.DirectoryPath, "Settings.a2t")

    Public Property MinimumStockPrice As Decimal
    Public Property MaximumStockPrice As Decimal
    Public Property MinimumATRPercentage As Decimal
    Public Property MinimumBlankCandlePercentage As Decimal
    Public Property NumberOfStock As Integer

    Public Property MainStockParallelHit As Integer
    Public Property OptionStockParallelHit As Integer
    Public Property RatioLowerValue As Decimal
    Public Property RatioUpperValue As Decimal
    Public Property OnlyWithCurrentExpiryContracts As Boolean
End Class
