<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoadComponentForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LoadComponentForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.FormatLabel = New System.Windows.Forms.Label()
        Me.KCVTextBox = New System.Windows.Forms.TextBox()
        Me.KCVLabel = New System.Windows.Forms.Label()
        Me.ComponentTextBox = New System.Windows.Forms.TextBox()
        Me.ComponentLabel = New System.Windows.Forms.Label()
        Me.LoadComponentButton = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.FormatLabel)
        Me.GroupBox1.Controls.Add(Me.KCVTextBox)
        Me.GroupBox1.Controls.Add(Me.KCVLabel)
        Me.GroupBox1.Controls.Add(Me.ComponentTextBox)
        Me.GroupBox1.Controls.Add(Me.ComponentLabel)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 23)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(349, 115)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Componente"
        '
        'FormatLabel
        '
        Me.FormatLabel.AutoSize = True
        Me.FormatLabel.ForeColor = System.Drawing.Color.Red
        Me.FormatLabel.Location = New System.Drawing.Point(91, 64)
        Me.FormatLabel.Name = "FormatLabel"
        Me.FormatLabel.Size = New System.Drawing.Size(0, 13)
        Me.FormatLabel.TabIndex = 5
        '
        'KCVTextBox
        '
        Me.KCVTextBox.Location = New System.Drawing.Point(94, 86)
        Me.KCVTextBox.MaxLength = 6
        Me.KCVTextBox.Name = "KCVTextBox"
        Me.KCVTextBox.ReadOnly = True
        Me.KCVTextBox.Size = New System.Drawing.Size(52, 20)
        Me.KCVTextBox.TabIndex = 3
        '
        'KCVLabel
        '
        Me.KCVLabel.AutoSize = True
        Me.KCVLabel.Location = New System.Drawing.Point(57, 89)
        Me.KCVLabel.Name = "KCVLabel"
        Me.KCVLabel.Size = New System.Drawing.Size(31, 13)
        Me.KCVLabel.TabIndex = 2
        Me.KCVLabel.Text = "KCV:"
        '
        'ComponentTextBox
        '
        Me.ComponentTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ComponentTextBox.Location = New System.Drawing.Point(94, 41)
        Me.ComponentTextBox.MaxLength = 32
        Me.ComponentTextBox.Name = "ComponentTextBox"
        Me.ComponentTextBox.Size = New System.Drawing.Size(238, 20)
        Me.ComponentTextBox.TabIndex = 1
        '
        'ComponentLabel
        '
        Me.ComponentLabel.AutoSize = True
        Me.ComponentLabel.Location = New System.Drawing.Point(18, 44)
        Me.ComponentLabel.Name = "ComponentLabel"
        Me.ComponentLabel.Size = New System.Drawing.Size(70, 13)
        Me.ComponentLabel.TabIndex = 0
        Me.ComponentLabel.Text = "Componente:"
        '
        'LoadComponentButton
        '
        Me.LoadComponentButton.Location = New System.Drawing.Point(249, 144)
        Me.LoadComponentButton.Name = "LoadComponentButton"
        Me.LoadComponentButton.Size = New System.Drawing.Size(110, 27)
        Me.LoadComponentButton.TabIndex = 4
        Me.LoadComponentButton.Text = "Cargar Componente"
        Me.LoadComponentButton.UseVisualStyleBackColor = True
        '
        'LoadComponentForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(371, 180)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.LoadComponentButton)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "LoadComponentForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Cargar Componente"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents LoadComponentButton As System.Windows.Forms.Button
    Friend WithEvents KCVTextBox As System.Windows.Forms.TextBox
    Friend WithEvents KCVLabel As System.Windows.Forms.Label
    Friend WithEvents ComponentTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ComponentLabel As System.Windows.Forms.Label
    Friend WithEvents FormatLabel As System.Windows.Forms.Label
End Class
