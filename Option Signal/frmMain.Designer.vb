<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.grpMode = New System.Windows.Forms.GroupBox()
        Me.rdbWithoutAPI = New System.Windows.Forms.RadioButton()
        Me.rdbWithAPI = New System.Windows.Forms.RadioButton()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.sfdgvMain = New Syncfusion.WinForms.DataGrid.SfDataGrid()
        Me.lblDuration = New System.Windows.Forms.Label()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.grpMode.SuspendLayout()
        CType(Me.sfdgvMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpMode
        '
        Me.grpMode.Controls.Add(Me.rdbWithoutAPI)
        Me.grpMode.Controls.Add(Me.rdbWithAPI)
        Me.grpMode.Location = New System.Drawing.Point(3, 2)
        Me.grpMode.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.grpMode.Name = "grpMode"
        Me.grpMode.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.grpMode.Size = New System.Drawing.Size(207, 65)
        Me.grpMode.TabIndex = 28
        Me.grpMode.TabStop = False
        Me.grpMode.Text = "Mode"
        '
        'rdbWithoutAPI
        '
        Me.rdbWithoutAPI.AutoSize = True
        Me.rdbWithoutAPI.Checked = True
        Me.rdbWithoutAPI.Location = New System.Drawing.Point(93, 25)
        Me.rdbWithoutAPI.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.rdbWithoutAPI.Name = "rdbWithoutAPI"
        Me.rdbWithoutAPI.Size = New System.Drawing.Size(102, 21)
        Me.rdbWithoutAPI.TabIndex = 1
        Me.rdbWithoutAPI.TabStop = True
        Me.rdbWithoutAPI.Text = "Without API"
        Me.rdbWithoutAPI.UseVisualStyleBackColor = True
        '
        'rdbWithAPI
        '
        Me.rdbWithAPI.AutoSize = True
        Me.rdbWithAPI.Location = New System.Drawing.Point(5, 25)
        Me.rdbWithAPI.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.rdbWithAPI.Name = "rdbWithAPI"
        Me.rdbWithAPI.Size = New System.Drawing.Size(82, 21)
        Me.rdbWithAPI.TabIndex = 0
        Me.rdbWithAPI.Text = "With API"
        Me.rdbWithAPI.UseVisualStyleBackColor = True
        '
        'btnStop
        '
        Me.btnStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStop.Location = New System.Drawing.Point(1228, 16)
        Me.btnStop.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(139, 39)
        Me.btnStop.TabIndex = 30
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStart.Location = New System.Drawing.Point(1076, 17)
        Me.btnStart.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(145, 39)
        Me.btnStart.TabIndex = 29
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'lblProgress
        '
        Me.lblProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblProgress.Location = New System.Drawing.Point(0, 581)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(1448, 44)
        Me.lblProgress.TabIndex = 31
        Me.lblProgress.Text = "Progress Status"
        '
        'sfdgvMain
        '
        Me.sfdgvMain.AccessibleName = "Table"
        Me.sfdgvMain.AllowDraggingColumns = True
        Me.sfdgvMain.AllowEditing = False
        Me.sfdgvMain.AllowFiltering = True
        Me.sfdgvMain.AllowResizingColumns = True
        Me.sfdgvMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.sfdgvMain.AutoGenerateColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoGenerateColumnsMode.SmartReset
        Me.sfdgvMain.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.AllCells
        Me.sfdgvMain.Location = New System.Drawing.Point(3, 73)
        Me.sfdgvMain.Margin = New System.Windows.Forms.Padding(4)
        Me.sfdgvMain.Name = "sfdgvMain"
        Me.sfdgvMain.PasteOption = Syncfusion.WinForms.DataGrid.Enums.PasteOptions.None
        Me.sfdgvMain.Size = New System.Drawing.Size(1363, 504)
        Me.sfdgvMain.TabIndex = 35
        Me.sfdgvMain.Text = "SfDataGrid1"
        '
        'lblDuration
        '
        Me.lblDuration.AutoSize = True
        Me.lblDuration.Location = New System.Drawing.Point(237, 30)
        Me.lblDuration.Name = "lblDuration"
        Me.lblDuration.Size = New System.Drawing.Size(164, 17)
        Me.lblDuration.TabIndex = 36
        Me.lblDuration.Text = "Last Iteration Duration: 0"
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.Location = New System.Drawing.Point(460, 30)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(189, 17)
        Me.lblTime.TabIndex = 37
        Me.lblTime.Text = "Last Iteration Time: 00:00:00"
        '
        'btnSettings
        '
        Me.btnSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSettings.Location = New System.Drawing.Point(757, 17)
        Me.btnSettings.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(145, 39)
        Me.btnSettings.TabIndex = 38
        Me.btnSettings.Text = "Settings"
        Me.btnSettings.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1371, 629)
        Me.Controls.Add(Me.btnSettings)
        Me.Controls.Add(Me.lblTime)
        Me.Controls.Add(Me.lblDuration)
        Me.Controls.Add(Me.sfdgvMain)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.grpMode)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Signal Me"
        Me.grpMode.ResumeLayout(False)
        Me.grpMode.PerformLayout()
        CType(Me.sfdgvMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents grpMode As GroupBox
    Friend WithEvents rdbWithoutAPI As RadioButton
    Friend WithEvents rdbWithAPI As RadioButton
    Friend WithEvents btnStop As Button
    Friend WithEvents btnStart As Button
    Friend WithEvents lblProgress As Label
    Friend WithEvents lblFromDate As Label
    Friend WithEvents sfdgvMain As Syncfusion.WinForms.DataGrid.SfDataGrid
    Friend WithEvents lblDuration As Label
    Friend WithEvents lblTime As Label
    Friend WithEvents btnSettings As Button
End Class
