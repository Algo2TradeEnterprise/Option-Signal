<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSettings))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.btnSaveMomentumReversalSettings = New System.Windows.Forms.Button()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabOthers = New System.Windows.Forms.TabPage()
        Me.txtRatioUpperValue = New System.Windows.Forms.TextBox()
        Me.lblRatioUpperValue = New System.Windows.Forms.Label()
        Me.txtRatioLowerValue = New System.Windows.Forms.TextBox()
        Me.lblRatioLowerValue = New System.Windows.Forms.Label()
        Me.txtOptionStockParallelHit = New System.Windows.Forms.TextBox()
        Me.lblOptionStockParallelHit = New System.Windows.Forms.Label()
        Me.txtMainStockParallelHit = New System.Windows.Forms.TextBox()
        Me.lblMainStockParallelHit = New System.Windows.Forms.Label()
        Me.tabStockSelection = New System.Windows.Forms.TabPage()
        Me.txtBlankCandlePercentage = New System.Windows.Forms.TextBox()
        Me.lblBlankCandlePercentage = New System.Windows.Forms.Label()
        Me.txtNumberOfStock = New System.Windows.Forms.TextBox()
        Me.lblNumberOfStock = New System.Windows.Forms.Label()
        Me.txtATRPercentage = New System.Windows.Forms.TextBox()
        Me.lblATR = New System.Windows.Forms.Label()
        Me.txtMaxPrice = New System.Windows.Forms.TextBox()
        Me.lblMaxPrice = New System.Windows.Forms.Label()
        Me.txtMinPrice = New System.Windows.Forms.TextBox()
        Me.lblMinPrice = New System.Windows.Forms.Label()
        Me.lblOnlyWithCurrentExpiryContract = New System.Windows.Forms.Label()
        Me.chkbOnlyWithCurrentExpiryContract = New System.Windows.Forms.CheckBox()
        Me.tabMain.SuspendLayout()
        Me.tabOthers.SuspendLayout()
        Me.tabStockSelection.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "save-icon-36533.png")
        '
        'btnSaveMomentumReversalSettings
        '
        Me.btnSaveMomentumReversalSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveMomentumReversalSettings.ImageKey = "save-icon-36533.png"
        Me.btnSaveMomentumReversalSettings.ImageList = Me.ImageList1
        Me.btnSaveMomentumReversalSettings.Location = New System.Drawing.Point(478, 25)
        Me.btnSaveMomentumReversalSettings.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSaveMomentumReversalSettings.Name = "btnSaveMomentumReversalSettings"
        Me.btnSaveMomentumReversalSettings.Size = New System.Drawing.Size(112, 58)
        Me.btnSaveMomentumReversalSettings.TabIndex = 12
        Me.btnSaveMomentumReversalSettings.Text = "&Save"
        Me.btnSaveMomentumReversalSettings.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveMomentumReversalSettings.UseVisualStyleBackColor = True
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabOthers)
        Me.tabMain.Controls.Add(Me.tabStockSelection)
        Me.tabMain.Location = New System.Drawing.Point(1, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(470, 228)
        Me.tabMain.TabIndex = 13
        '
        'tabOthers
        '
        Me.tabOthers.Controls.Add(Me.chkbOnlyWithCurrentExpiryContract)
        Me.tabOthers.Controls.Add(Me.lblOnlyWithCurrentExpiryContract)
        Me.tabOthers.Controls.Add(Me.txtRatioUpperValue)
        Me.tabOthers.Controls.Add(Me.lblRatioUpperValue)
        Me.tabOthers.Controls.Add(Me.txtRatioLowerValue)
        Me.tabOthers.Controls.Add(Me.lblRatioLowerValue)
        Me.tabOthers.Controls.Add(Me.txtOptionStockParallelHit)
        Me.tabOthers.Controls.Add(Me.lblOptionStockParallelHit)
        Me.tabOthers.Controls.Add(Me.txtMainStockParallelHit)
        Me.tabOthers.Controls.Add(Me.lblMainStockParallelHit)
        Me.tabOthers.Location = New System.Drawing.Point(4, 25)
        Me.tabOthers.Name = "tabOthers"
        Me.tabOthers.Padding = New System.Windows.Forms.Padding(3)
        Me.tabOthers.Size = New System.Drawing.Size(462, 199)
        Me.tabOthers.TabIndex = 0
        Me.tabOthers.Text = "Others"
        Me.tabOthers.UseVisualStyleBackColor = True
        '
        'txtRatioUpperValue
        '
        Me.txtRatioUpperValue.Location = New System.Drawing.Point(192, 116)
        Me.txtRatioUpperValue.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRatioUpperValue.Name = "txtRatioUpperValue"
        Me.txtRatioUpperValue.Size = New System.Drawing.Size(201, 22)
        Me.txtRatioUpperValue.TabIndex = 61
        Me.txtRatioUpperValue.Tag = "Ratio Upper Value"
        '
        'lblRatioUpperValue
        '
        Me.lblRatioUpperValue.AutoSize = True
        Me.lblRatioUpperValue.Location = New System.Drawing.Point(11, 119)
        Me.lblRatioUpperValue.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRatioUpperValue.Name = "lblRatioUpperValue"
        Me.lblRatioUpperValue.Size = New System.Drawing.Size(124, 17)
        Me.lblRatioUpperValue.TabIndex = 65
        Me.lblRatioUpperValue.Text = "Ratio Upper Value"
        '
        'txtRatioLowerValue
        '
        Me.txtRatioLowerValue.Location = New System.Drawing.Point(192, 81)
        Me.txtRatioLowerValue.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRatioLowerValue.Name = "txtRatioLowerValue"
        Me.txtRatioLowerValue.Size = New System.Drawing.Size(201, 22)
        Me.txtRatioLowerValue.TabIndex = 60
        Me.txtRatioLowerValue.Tag = "Ratio Lower Value"
        '
        'lblRatioLowerValue
        '
        Me.lblRatioLowerValue.AutoSize = True
        Me.lblRatioLowerValue.Location = New System.Drawing.Point(10, 84)
        Me.lblRatioLowerValue.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRatioLowerValue.Name = "lblRatioLowerValue"
        Me.lblRatioLowerValue.Size = New System.Drawing.Size(123, 17)
        Me.lblRatioLowerValue.TabIndex = 64
        Me.lblRatioLowerValue.Text = "Ratio Lower Value"
        '
        'txtOptionStockParallelHit
        '
        Me.txtOptionStockParallelHit.Location = New System.Drawing.Point(192, 46)
        Me.txtOptionStockParallelHit.Margin = New System.Windows.Forms.Padding(4)
        Me.txtOptionStockParallelHit.Name = "txtOptionStockParallelHit"
        Me.txtOptionStockParallelHit.Size = New System.Drawing.Size(201, 22)
        Me.txtOptionStockParallelHit.TabIndex = 59
        Me.txtOptionStockParallelHit.Tag = "Option Stock Parallel Hit"
        '
        'lblOptionStockParallelHit
        '
        Me.lblOptionStockParallelHit.AutoSize = True
        Me.lblOptionStockParallelHit.Location = New System.Drawing.Point(10, 50)
        Me.lblOptionStockParallelHit.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOptionStockParallelHit.Name = "lblOptionStockParallelHit"
        Me.lblOptionStockParallelHit.Size = New System.Drawing.Size(161, 17)
        Me.lblOptionStockParallelHit.TabIndex = 63
        Me.lblOptionStockParallelHit.Text = "Option Stock Parallel Hit"
        '
        'txtMainStockParallelHit
        '
        Me.txtMainStockParallelHit.Location = New System.Drawing.Point(192, 12)
        Me.txtMainStockParallelHit.Margin = New System.Windows.Forms.Padding(4)
        Me.txtMainStockParallelHit.Name = "txtMainStockParallelHit"
        Me.txtMainStockParallelHit.Size = New System.Drawing.Size(201, 22)
        Me.txtMainStockParallelHit.TabIndex = 58
        Me.txtMainStockParallelHit.Tag = "Main Stock Parallel Hit"
        '
        'lblMainStockParallelHit
        '
        Me.lblMainStockParallelHit.AutoSize = True
        Me.lblMainStockParallelHit.Location = New System.Drawing.Point(10, 15)
        Me.lblMainStockParallelHit.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMainStockParallelHit.Name = "lblMainStockParallelHit"
        Me.lblMainStockParallelHit.Size = New System.Drawing.Size(149, 17)
        Me.lblMainStockParallelHit.TabIndex = 62
        Me.lblMainStockParallelHit.Text = "Main Stock Parallel Hit"
        '
        'tabStockSelection
        '
        Me.tabStockSelection.Controls.Add(Me.txtBlankCandlePercentage)
        Me.tabStockSelection.Controls.Add(Me.lblBlankCandlePercentage)
        Me.tabStockSelection.Controls.Add(Me.txtNumberOfStock)
        Me.tabStockSelection.Controls.Add(Me.lblNumberOfStock)
        Me.tabStockSelection.Controls.Add(Me.txtATRPercentage)
        Me.tabStockSelection.Controls.Add(Me.lblATR)
        Me.tabStockSelection.Controls.Add(Me.txtMaxPrice)
        Me.tabStockSelection.Controls.Add(Me.lblMaxPrice)
        Me.tabStockSelection.Controls.Add(Me.txtMinPrice)
        Me.tabStockSelection.Controls.Add(Me.lblMinPrice)
        Me.tabStockSelection.Location = New System.Drawing.Point(4, 25)
        Me.tabStockSelection.Name = "tabStockSelection"
        Me.tabStockSelection.Padding = New System.Windows.Forms.Padding(3)
        Me.tabStockSelection.Size = New System.Drawing.Size(462, 199)
        Me.tabStockSelection.TabIndex = 1
        Me.tabStockSelection.Text = "Stock Selection"
        Me.tabStockSelection.UseVisualStyleBackColor = True
        '
        'txtBlankCandlePercentage
        '
        Me.txtBlankCandlePercentage.Location = New System.Drawing.Point(191, 117)
        Me.txtBlankCandlePercentage.Margin = New System.Windows.Forms.Padding(4)
        Me.txtBlankCandlePercentage.Name = "txtBlankCandlePercentage"
        Me.txtBlankCandlePercentage.Size = New System.Drawing.Size(201, 22)
        Me.txtBlankCandlePercentage.TabIndex = 50
        Me.txtBlankCandlePercentage.Tag = "Blank Candle %"
        '
        'lblBlankCandlePercentage
        '
        Me.lblBlankCandlePercentage.AutoSize = True
        Me.lblBlankCandlePercentage.Location = New System.Drawing.Point(10, 120)
        Me.lblBlankCandlePercentage.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblBlankCandlePercentage.Name = "lblBlankCandlePercentage"
        Me.lblBlankCandlePercentage.Size = New System.Drawing.Size(169, 17)
        Me.lblBlankCandlePercentage.TabIndex = 57
        Me.lblBlankCandlePercentage.Text = "Maximum Blank Candle %"
        '
        'txtNumberOfStock
        '
        Me.txtNumberOfStock.Location = New System.Drawing.Point(191, 152)
        Me.txtNumberOfStock.Margin = New System.Windows.Forms.Padding(4)
        Me.txtNumberOfStock.Name = "txtNumberOfStock"
        Me.txtNumberOfStock.Size = New System.Drawing.Size(201, 22)
        Me.txtNumberOfStock.TabIndex = 51
        Me.txtNumberOfStock.Tag = "Number Of Stock"
        '
        'lblNumberOfStock
        '
        Me.lblNumberOfStock.AutoSize = True
        Me.lblNumberOfStock.Location = New System.Drawing.Point(9, 155)
        Me.lblNumberOfStock.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNumberOfStock.Name = "lblNumberOfStock"
        Me.lblNumberOfStock.Size = New System.Drawing.Size(116, 17)
        Me.lblNumberOfStock.TabIndex = 56
        Me.lblNumberOfStock.Text = "Number Of Stock"
        '
        'txtATRPercentage
        '
        Me.txtATRPercentage.Location = New System.Drawing.Point(191, 82)
        Me.txtATRPercentage.Margin = New System.Windows.Forms.Padding(4)
        Me.txtATRPercentage.Name = "txtATRPercentage"
        Me.txtATRPercentage.Size = New System.Drawing.Size(201, 22)
        Me.txtATRPercentage.TabIndex = 48
        Me.txtATRPercentage.Tag = "ATR %"
        '
        'lblATR
        '
        Me.lblATR.AutoSize = True
        Me.lblATR.Location = New System.Drawing.Point(9, 85)
        Me.lblATR.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblATR.Name = "lblATR"
        Me.lblATR.Size = New System.Drawing.Size(111, 17)
        Me.lblATR.TabIndex = 54
        Me.lblATR.Text = "Minimum ATR %"
        '
        'txtMaxPrice
        '
        Me.txtMaxPrice.Location = New System.Drawing.Point(191, 47)
        Me.txtMaxPrice.Margin = New System.Windows.Forms.Padding(4)
        Me.txtMaxPrice.Name = "txtMaxPrice"
        Me.txtMaxPrice.Size = New System.Drawing.Size(201, 22)
        Me.txtMaxPrice.TabIndex = 47
        Me.txtMaxPrice.Tag = "Max Price"
        '
        'lblMaxPrice
        '
        Me.lblMaxPrice.AutoSize = True
        Me.lblMaxPrice.Location = New System.Drawing.Point(9, 51)
        Me.lblMaxPrice.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMaxPrice.Name = "lblMaxPrice"
        Me.lblMaxPrice.Size = New System.Drawing.Size(102, 17)
        Me.lblMaxPrice.TabIndex = 53
        Me.lblMaxPrice.Text = "Maximum Price"
        '
        'txtMinPrice
        '
        Me.txtMinPrice.Location = New System.Drawing.Point(191, 13)
        Me.txtMinPrice.Margin = New System.Windows.Forms.Padding(4)
        Me.txtMinPrice.Name = "txtMinPrice"
        Me.txtMinPrice.Size = New System.Drawing.Size(201, 22)
        Me.txtMinPrice.TabIndex = 46
        Me.txtMinPrice.Tag = "Min Price"
        '
        'lblMinPrice
        '
        Me.lblMinPrice.AutoSize = True
        Me.lblMinPrice.Location = New System.Drawing.Point(9, 16)
        Me.lblMinPrice.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMinPrice.Name = "lblMinPrice"
        Me.lblMinPrice.Size = New System.Drawing.Size(99, 17)
        Me.lblMinPrice.TabIndex = 52
        Me.lblMinPrice.Text = "Minimum Price"
        '
        'lblOnlyWithCurrentExpiryContract
        '
        Me.lblOnlyWithCurrentExpiryContract.AutoSize = True
        Me.lblOnlyWithCurrentExpiryContract.Location = New System.Drawing.Point(10, 153)
        Me.lblOnlyWithCurrentExpiryContract.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOnlyWithCurrentExpiryContract.Name = "lblOnlyWithCurrentExpiryContract"
        Me.lblOnlyWithCurrentExpiryContract.Size = New System.Drawing.Size(219, 17)
        Me.lblOnlyWithCurrentExpiryContract.TabIndex = 66
        Me.lblOnlyWithCurrentExpiryContract.Text = "Only With Current Expiry Contract"
        '
        'chkbOnlyWithCurrentExpiryContract
        '
        Me.chkbOnlyWithCurrentExpiryContract.AutoSize = True
        Me.chkbOnlyWithCurrentExpiryContract.Location = New System.Drawing.Point(259, 153)
        Me.chkbOnlyWithCurrentExpiryContract.Name = "chkbOnlyWithCurrentExpiryContract"
        Me.chkbOnlyWithCurrentExpiryContract.Size = New System.Drawing.Size(18, 17)
        Me.chkbOnlyWithCurrentExpiryContract.TabIndex = 67
        Me.chkbOnlyWithCurrentExpiryContract.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(600, 228)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.btnSaveMomentumReversalSettings)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Settings"
        Me.tabMain.ResumeLayout(False)
        Me.tabOthers.ResumeLayout(False)
        Me.tabOthers.PerformLayout()
        Me.tabStockSelection.ResumeLayout(False)
        Me.tabStockSelection.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents btnSaveMomentumReversalSettings As Button
    Friend WithEvents tabMain As TabControl
    Friend WithEvents tabOthers As TabPage
    Friend WithEvents tabStockSelection As TabPage
    Friend WithEvents txtRatioUpperValue As TextBox
    Friend WithEvents lblRatioUpperValue As Label
    Friend WithEvents txtRatioLowerValue As TextBox
    Friend WithEvents lblRatioLowerValue As Label
    Friend WithEvents txtOptionStockParallelHit As TextBox
    Friend WithEvents lblOptionStockParallelHit As Label
    Friend WithEvents txtMainStockParallelHit As TextBox
    Friend WithEvents lblMainStockParallelHit As Label
    Friend WithEvents txtBlankCandlePercentage As TextBox
    Friend WithEvents lblBlankCandlePercentage As Label
    Friend WithEvents txtNumberOfStock As TextBox
    Friend WithEvents lblNumberOfStock As Label
    Friend WithEvents txtATRPercentage As TextBox
    Friend WithEvents lblATR As Label
    Friend WithEvents txtMaxPrice As TextBox
    Friend WithEvents lblMaxPrice As Label
    Friend WithEvents txtMinPrice As TextBox
    Friend WithEvents lblMinPrice As Label
    Friend WithEvents lblOnlyWithCurrentExpiryContract As Label
    Friend WithEvents chkbOnlyWithCurrentExpiryContract As CheckBox
End Class
