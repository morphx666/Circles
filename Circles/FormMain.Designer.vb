<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
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
        Me.ButtonReset = New System.Windows.Forms.Button()
        Me.CheckBoxSpeedGraphs = New System.Windows.Forms.CheckBox()
        Me.CheckBoxSimulatePockets = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBoxOptions = New System.Windows.Forms.GroupBox()
        Me.ButtonRun = New System.Windows.Forms.Button()
        Me.RadioButtonNewtonsCradle = New System.Windows.Forms.RadioButton()
        Me.RadioButtonPoolSimulation = New System.Windows.Forms.RadioButton()
        Me.GroupBoxOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonReset
        '
        Me.ButtonReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonReset.Location = New System.Drawing.Point(539, 609)
        Me.ButtonReset.Name = "ButtonReset"
        Me.ButtonReset.Size = New System.Drawing.Size(124, 45)
        Me.ButtonReset.TabIndex = 1
        Me.ButtonReset.Text = "Reset"
        Me.ButtonReset.UseVisualStyleBackColor = True
        '
        'CheckBoxSpeedGraphs
        '
        Me.CheckBoxSpeedGraphs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxSpeedGraphs.AutoSize = True
        Me.CheckBoxSpeedGraphs.BackColor = System.Drawing.Color.Transparent
        Me.CheckBoxSpeedGraphs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxSpeedGraphs.ForeColor = System.Drawing.Color.White
        Me.CheckBoxSpeedGraphs.Location = New System.Drawing.Point(539, 586)
        Me.CheckBoxSpeedGraphs.Name = "CheckBoxSpeedGraphs"
        Me.CheckBoxSpeedGraphs.Size = New System.Drawing.Size(106, 17)
        Me.CheckBoxSpeedGraphs.TabIndex = 2
        Me.CheckBoxSpeedGraphs.Text = "Speed Graphs"
        Me.CheckBoxSpeedGraphs.UseVisualStyleBackColor = False
        '
        'CheckBoxSimulatePockets
        '
        Me.CheckBoxSimulatePockets.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxSimulatePockets.AutoSize = True
        Me.CheckBoxSimulatePockets.BackColor = System.Drawing.Color.Transparent
        Me.CheckBoxSimulatePockets.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxSimulatePockets.ForeColor = System.Drawing.Color.White
        Me.CheckBoxSimulatePockets.Location = New System.Drawing.Point(539, 563)
        Me.CheckBoxSimulatePockets.Name = "CheckBoxSimulatePockets"
        Me.CheckBoxSimulatePockets.Size = New System.Drawing.Size(124, 17)
        Me.CheckBoxSimulatePockets.TabIndex = 3
        Me.CheckBoxSimulatePockets.Text = "Simulate Pockets"
        Me.CheckBoxSimulatePockets.UseVisualStyleBackColor = False
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
        'GroupBoxOptions
        '
        Me.GroupBoxOptions.Controls.Add(Me.ButtonRun)
        Me.GroupBoxOptions.Controls.Add(Me.RadioButtonNewtonsCradle)
        Me.GroupBoxOptions.Controls.Add(Me.RadioButtonPoolSimulation)
        Me.GroupBoxOptions.ForeColor = System.Drawing.Color.White
        Me.GroupBoxOptions.Location = New System.Drawing.Point(218, 250)
        Me.GroupBoxOptions.Name = "GroupBoxOptions"
        Me.GroupBoxOptions.Size = New System.Drawing.Size(229, 112)
        Me.GroupBoxOptions.TabIndex = 5
        Me.GroupBoxOptions.TabStop = False
        Me.GroupBoxOptions.Text = "Reset As"
        '
        'ButtonRun
        '
        Me.ButtonRun.ForeColor = System.Drawing.Color.Black
        Me.ButtonRun.Location = New System.Drawing.Point(148, 83)
        Me.ButtonRun.Name = "ButtonRun"
        Me.ButtonRun.Size = New System.Drawing.Size(75, 23)
        Me.ButtonRun.TabIndex = 2
        Me.ButtonRun.Text = "Run"
        Me.ButtonRun.UseVisualStyleBackColor = True
        '
        'RadioButtonNewtonsCradle
        '
        Me.RadioButtonNewtonsCradle.AutoSize = True
        Me.RadioButtonNewtonsCradle.ForeColor = System.Drawing.Color.White
        Me.RadioButtonNewtonsCradle.Location = New System.Drawing.Point(33, 49)
        Me.RadioButtonNewtonsCradle.Name = "RadioButtonNewtonsCradle"
        Me.RadioButtonNewtonsCradle.Size = New System.Drawing.Size(102, 17)
        Me.RadioButtonNewtonsCradle.TabIndex = 1
        Me.RadioButtonNewtonsCradle.Text = "Newton's Cradle"
        Me.RadioButtonNewtonsCradle.UseVisualStyleBackColor = True
        '
        'RadioButtonPoolSimulation
        '
        Me.RadioButtonPoolSimulation.AutoSize = True
        Me.RadioButtonPoolSimulation.Checked = True
        Me.RadioButtonPoolSimulation.ForeColor = System.Drawing.Color.White
        Me.RadioButtonPoolSimulation.Location = New System.Drawing.Point(33, 26)
        Me.RadioButtonPoolSimulation.Name = "RadioButtonPoolSimulation"
        Me.RadioButtonPoolSimulation.Size = New System.Drawing.Size(127, 17)
        Me.RadioButtonPoolSimulation.TabIndex = 0
        Me.RadioButtonPoolSimulation.TabStop = True
        Me.RadioButtonPoolSimulation.Text = "Pool Table Simulation"
        Me.RadioButtonPoolSimulation.UseVisualStyleBackColor = True
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(675, 666)
        Me.Controls.Add(Me.GroupBoxOptions)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CheckBoxSimulatePockets)
        Me.Controls.Add(Me.CheckBoxSpeedGraphs)
        Me.Controls.Add(Me.ButtonReset)
        Me.KeyPreview = True
        Me.Name = "FormMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Newton's Conservation of Momentum"
        Me.GroupBoxOptions.ResumeLayout(False)
        Me.GroupBoxOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonReset As System.Windows.Forms.Button
    Friend WithEvents CheckBoxSpeedGraphs As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxSimulatePockets As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBoxOptions As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonRun As System.Windows.Forms.Button
    Friend WithEvents RadioButtonNewtonsCradle As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonPoolSimulation As System.Windows.Forms.RadioButton

End Class
