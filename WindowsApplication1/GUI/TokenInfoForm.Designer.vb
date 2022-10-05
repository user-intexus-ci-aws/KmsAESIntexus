<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TokenInfoForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TokenInfoForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TokenPassphraseTextBox = New System.Windows.Forms.TextBox()
        Me.SendTokenPassphraseButton = New System.Windows.Forms.Button()
        Me.CancelTokenPassphraseButton = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TokenPassphraseTextBox)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(231, 55)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Token passphrase"
        '
        'TokenPassphraseTextBox
        '
        Me.TokenPassphraseTextBox.Location = New System.Drawing.Point(6, 19)
        Me.TokenPassphraseTextBox.Name = "TokenPassphraseTextBox"
        Me.TokenPassphraseTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(8226)
        Me.TokenPassphraseTextBox.Size = New System.Drawing.Size(219, 20)
        Me.TokenPassphraseTextBox.TabIndex = 0
        '
        'SendTokenPassphraseButton
        '
        Me.SendTokenPassphraseButton.Location = New System.Drawing.Point(142, 73)
        Me.SendTokenPassphraseButton.Name = "SendTokenPassphraseButton"
        Me.SendTokenPassphraseButton.Size = New System.Drawing.Size(101, 22)
        Me.SendTokenPassphraseButton.TabIndex = 1
        Me.SendTokenPassphraseButton.Text = "Enviar"
        Me.SendTokenPassphraseButton.UseVisualStyleBackColor = True
        '
        'CancelTokenPassphraseButton
        '
        Me.CancelTokenPassphraseButton.Location = New System.Drawing.Point(18, 73)
        Me.CancelTokenPassphraseButton.Name = "CancelTokenPassphraseButton"
        Me.CancelTokenPassphraseButton.Size = New System.Drawing.Size(101, 22)
        Me.CancelTokenPassphraseButton.TabIndex = 2
        Me.CancelTokenPassphraseButton.Text = "Cancelar"
        Me.CancelTokenPassphraseButton.UseVisualStyleBackColor = True
        '
        'TokenInfoForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(255, 102)
        Me.Controls.Add(Me.CancelTokenPassphraseButton)
        Me.Controls.Add(Me.SendTokenPassphraseButton)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "TokenInfoForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Token Passphrase"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TokenPassphraseTextBox As System.Windows.Forms.TextBox
    Friend WithEvents SendTokenPassphraseButton As System.Windows.Forms.Button
    Friend WithEvents CancelTokenPassphraseButton As System.Windows.Forms.Button
End Class
