<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.chkSpeedGraphs = New System.Windows.Forms.CheckBox()
        Me.chkSimulatePockets = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gbOptions = New System.Windows.Forms.GroupBox()
        Me.btnRun = New System.Windows.Forms.Button()
        Me.rbNewtonsCradle = New System.Windows.Forms.RadioButton()
        Me.rbPoolSimulation = New System.Windows.Forms.RadioButton()
        Me.gbOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnReset
        '
        Me.btnReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReset.Location = New System.Drawing.Point(539, 609)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(124, 45)
        Me.btnReset.TabIndex = 1
        Me.btnReset.Text = "Reset"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'chkSpeedGraphs
        '
        Me.chkSpeedGraphs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkSpeedGraphs.AutoSize = True
        Me.chkSpeedGraphs.BackColor = System.Drawing.Color.Transparent
        Me.chkSpeedGraphs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSpeedGraphs.ForeColor = System.Drawing.Color.White
        Me.chkSpeedGraphs.Location = New System.Drawing.Point(539, 586)
        Me.chkSpeedGraphs.Name = "chkSpeedGraphs"
        Me.chkSpeedGraphs.Size = New System.Drawing.Size(106, 17)
        Me.chkSpeedGraphs.TabIndex = 2
        Me.chkSpeedGraphs.Text = "Speed Graphs"
        Me.chkSpeedGraphs.UseVisualStyleBackColor = False
        '
        'chkSimulatePockets
        '
        Me.chkSimulatePockets.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkSimulatePockets.AutoSize = True
        Me.chkSimulatePockets.BackColor = System.Drawing.Color.Transparent
        Me.chkSimulatePockets.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSimulatePockets.ForeColor = System.Drawing.Color.White
        Me.chkSimulatePockets.Location = New System.Drawing.Point(539, 563)
        Me.chkSimulatePockets.Name = "chkSimulatePockets"
        Me.chkSimulatePockets.Size = New System.Drawing.Size(124, 17)
        Me.chkSimulatePockets.TabIndex = 3
        Me.chkSimulatePockets.Text = "Simulate Pockets"
        Me.chkSimulatePockets.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(12, 628)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(174, 26)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "• Click and Drag to throw ball" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "• Right-Click and Drag to move ball"
        '
        'gbOptions
        '
        Me.gbOptions.Controls.Add(Me.btnRun)
        Me.gbOptions.Controls.Add(Me.rbNewtonsCradle)
        Me.gbOptions.Controls.Add(Me.rbPoolSimulation)
        Me.gbOptions.ForeColor = System.Drawing.Color.White
        Me.gbOptions.Location = New System.Drawing.Point(218, 250)
        Me.gbOptions.Name = "gbOptions"
        Me.gbOptions.Size = New System.Drawing.Size(229, 112)
        Me.gbOptions.TabIndex = 5
        Me.gbOptions.TabStop = False
        Me.gbOptions.Text = "Reset As"
        '
        'btnRun
        '
        Me.btnRun.ForeColor = System.Drawing.Color.Black
        Me.btnRun.Location = New System.Drawing.Point(148, 83)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(75, 23)
        Me.btnRun.TabIndex = 2
        Me.btnRun.Text = "Run"
        Me.btnRun.UseVisualStyleBackColor = True
        '
        'rbNewtonsCradle
        '
        Me.rbNewtonsCradle.AutoSize = True
        Me.rbNewtonsCradle.ForeColor = System.Drawing.Color.White
        Me.rbNewtonsCradle.Location = New System.Drawing.Point(33, 49)
        Me.rbNewtonsCradle.Name = "rbNewtonsCradle"
        Me.rbNewtonsCradle.Size = New System.Drawing.Size(102, 17)
        Me.rbNewtonsCradle.TabIndex = 1
        Me.rbNewtonsCradle.Text = "Newton's Cradle"
        Me.rbNewtonsCradle.UseVisualStyleBackColor = True
        '
        'rbPoolSimulation
        '
        Me.rbPoolSimulation.AutoSize = True
        Me.rbPoolSimulation.Checked = True
        Me.rbPoolSimulation.ForeColor = System.Drawing.Color.White
        Me.rbPoolSimulation.Location = New System.Drawing.Point(33, 26)
        Me.rbPoolSimulation.Name = "rbPoolSimulation"
        Me.rbPoolSimulation.Size = New System.Drawing.Size(127, 17)
        Me.rbPoolSimulation.TabIndex = 0
        Me.rbPoolSimulation.TabStop = True
        Me.rbPoolSimulation.Text = "Pool Table Simulation"
        Me.rbPoolSimulation.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(675, 666)
        Me.Controls.Add(Me.gbOptions)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkSimulatePockets)
        Me.Controls.Add(Me.chkSpeedGraphs)
        Me.Controls.Add(Me.btnReset)
        Me.Name = "frmMain"
        Me.Text = "Newton's Conservation of Momentum"
        Me.gbOptions.ResumeLayout(False)
        Me.gbOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnReset As System.Windows.Forms.Button
    Friend WithEvents chkSpeedGraphs As System.Windows.Forms.CheckBox
    Friend WithEvents chkSimulatePockets As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gbOptions As System.Windows.Forms.GroupBox
    Friend WithEvents btnRun As System.Windows.Forms.Button
    Friend WithEvents rbNewtonsCradle As System.Windows.Forms.RadioButton
    Friend WithEvents rbPoolSimulation As System.Windows.Forms.RadioButton

End Class
