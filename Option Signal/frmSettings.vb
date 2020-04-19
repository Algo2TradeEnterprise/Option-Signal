Imports System.IO
Public Class frmSettings
    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If File.Exists(SignalSettings.SettingsFilename) Then
            Dim settings As SignalSettings = Utilities.Strings.DeserializeToCollection(Of SignalSettings)(SignalSettings.SettingsFilename)

            txtMinPrice.Text = settings.MinimumStockPrice
            txtMaxPrice.Text = settings.MaximumStockPrice
            txtATRPercentage.Text = settings.MinimumATRPercentage
            txtBlankCandlePercentage.Text = settings.MinimumBlankCandlePercentage
            txtNumberOfStock.Text = settings.NumberOfStock

            txtMainStockParallelHit.Text = settings.MainStockParallelHit
            txtOptionStockParallelHit.Text = settings.OptionStockParallelHit
            txtRatioLowerValue.Text = settings.RatioLowerValue
            txtRatioUpperValue.Text = settings.RatioUpperValue
            chkbOnlyWithCurrentExpiryContract.Checked = settings.OnlyWithCurrentExpiryContracts
        End If
    End Sub

    Private Sub btnSaveMomentumReversalSettings_Click(sender As Object, e As EventArgs) Handles btnSaveMomentumReversalSettings.Click
        Try
            ValidateInputs()
            SaveSettings()
            Me.Close()
        Catch ex As Exception
            MessageBox.Show(String.Format("The following error occurred: {0}", ex.Message), "Settings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SaveSettings()
        Dim settings As SignalSettings = New SignalSettings

        settings.MinimumStockPrice = txtMinPrice.Text
        settings.MaximumStockPrice = txtMaxPrice.Text
        settings.MinimumATRPercentage = txtATRPercentage.Text
        settings.MinimumBlankCandlePercentage = txtBlankCandlePercentage.Text
        settings.NumberOfStock = txtNumberOfStock.Text

        settings.MainStockParallelHit = txtMainStockParallelHit.Text
        settings.OptionStockParallelHit = txtOptionStockParallelHit.Text
        settings.RatioLowerValue = txtRatioLowerValue.Text
        settings.RatioUpperValue = txtRatioUpperValue.Text
        settings.OnlyWithCurrentExpiryContracts = chkbOnlyWithCurrentExpiryContract.Checked

        Utilities.Strings.SerializeFromCollection(Of SignalSettings)(SignalSettings.SettingsFilename, settings)
    End Sub

    Private Function ValidateNumbers(ByVal startNumber As Decimal, ByVal endNumber As Decimal, ByVal inputTB As TextBox, Optional ByVal validateInteger As Boolean = False) As Boolean
        Dim ret As Boolean = False
        If IsNumeric(inputTB.Text) Then
            If validateInteger Then
                If Val(inputTB.Text) <> Math.Round(Val(inputTB.Text), 0) Then
                    Throw New ApplicationException(String.Format("{0} should be of type Integer", inputTB.Tag))
                End If
            End If
            If Val(inputTB.Text) >= startNumber And Val(inputTB.Text) <= endNumber Then
                ret = True
            End If
        End If
        If Not ret Then Throw New ApplicationException(String.Format("{0} cannot have a value < {1} or > {2}", inputTB.Tag, startNumber, endNumber))
        Return ret
    End Function

    Private Sub ValidateInputs()
        ValidateNumbers(0, Decimal.MaxValue, txtMinPrice)
        ValidateNumbers(0, Decimal.MaxValue, txtMaxPrice)
        ValidateNumbers(0, 100, txtATRPercentage)
        ValidateNumbers(0, 100, txtBlankCandlePercentage)
        ValidateNumbers(0, Integer.MaxValue, txtNumberOfStock, True)

        ValidateNumbers(0, Integer.MaxValue, txtMainStockParallelHit, True)
        ValidateNumbers(0, Integer.MaxValue, txtOptionStockParallelHit, True)
        ValidateNumbers(0, Decimal.MaxValue, txtRatioLowerValue)
        ValidateNumbers(0, Decimal.MaxValue, txtRatioUpperValue)
    End Sub
End Class