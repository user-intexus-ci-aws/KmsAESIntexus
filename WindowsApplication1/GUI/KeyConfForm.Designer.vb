<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class KeyConfForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(KeyConfForm))
        Me.KeyGroupBox = New System.Windows.Forms.GroupBox()
        Me.TokenNameTextBox = New System.Windows.Forms.TextBox()
        Me.TokenNameLabel = New System.Windows.Forms.Label()
        Me.KeyNameTextBox = New System.Windows.Forms.TextBox()
        Me.KeyNameLabel = New System.Windows.Forms.Label()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.KeyGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'KeyGroupBox
        '
        Me.KeyGroupBox.Controls.Add(Me.TokenNameTextBox)
        Me.KeyGroupBox.Controls.Add(Me.TokenNameLabel)
        Me.KeyGroupBox.Controls.Add(Me.KeyNameTextBox)
        Me.KeyGroupBox.Controls.Add(Me.KeyNameLabel)
        Me.KeyGroupBox.Location = New System.Drawing.Point(13, 17)
        Me.KeyGroupBox.Name = "KeyGroupBox"
        Me.KeyGroupBox.Size = New System.Drawing.Size(323, 92)
        Me.KeyGroupBox.TabIndex = 17
        Me.KeyGroupBox.TabStop = False
        Me.KeyGroupBox.Text = "Información de llaves"
        '
        'TokenNameTextBox
        '
        Me.TokenNameTextBox.Location = New System.Drawing.Point(125, 55)
        Me.TokenNameTextBox.Name = "TokenNameTextBox"
        Me.TokenNameTextBox.Size = New System.Drawing.Size(192, 20)
        Me.TokenNameTextBox.TabIndex = 14
        '
        'TokenNameLabel
        '
        Me.TokenNameLabel.AutoSize = True
        Me.TokenNameLabel.Location = New System.Drawing.Point(38, 62)
        Me.TokenNameLabel.Name = "TokenNameLabel"
        Me.TokenNameLabel.Size = New System.Drawing.Size(77, 13)
        Me.TokenNameLabel.TabIndex = 13
        Me.TokenNameLabel.Text = "Nombre token:"
        '
        'KeyNameTextBox
        '
        Me.KeyNameTextBox.Location = New System.Drawing.Point(125, 29)
        Me.KeyNameTextBox.Name = "KeyNameTextBox"
        Me.KeyNameTextBox.Size = New System.Drawing.Size(192, 20)
        Me.KeyNameTextBox.TabIndex = 12
        '
        'KeyNameLabel
        '
        Me.KeyNameLabel.AutoSize = True
        Me.KeyNameLabel.Location = New System.Drawing.Point(43, 32)
        Me.KeyNameLabel.Name = "KeyNameLabel"
        Me.KeyNameLabel.Size = New System.Drawing.Size(72, 13)
        Me.KeyNameLabel.TabIndex = 11
        Me.KeyNameLabel.Text = "Nombre llave:"
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(223, 115)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(113, 26)
        Me.SaveButton.TabIndex = 18
        Me.SaveButton.Text = "Guardar"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'KeyConfForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(353, 150)
        Me.Controls.Add(Me.KeyGroupBox)
        Me.Controls.Add(Me.SaveButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "KeyConfForm"
        Me.Text = "Configuración de llave"
        Me.KeyGroupBox.ResumeLayout(False)
        Me.KeyGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents KeyGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents TokenNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents TokenNameLabel As System.Windows.Forms.Label
    Friend WithEvents KeyNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents KeyNameLabel As System.Windows.Forms.Label
    Friend WithEvents SaveButton As System.Windows.Forms.Button
End Class
