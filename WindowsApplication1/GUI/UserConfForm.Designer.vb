<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UserConfForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UserConfForm))
        Me.UserGroupBox = New System.Windows.Forms.GroupBox()
        Me.PwTextBox = New System.Windows.Forms.TextBox()
        Me.PwLabel = New System.Windows.Forms.Label()
        Me.Pw2TextBox = New System.Windows.Forms.TextBox()
        Me.Pw1TextBox = New System.Windows.Forms.TextBox()
        Me.UserTextBox = New System.Windows.Forms.TextBox()
        Me.Pw2Label = New System.Windows.Forms.Label()
        Me.Pw1Label = New System.Windows.Forms.Label()
        Me.UserLabel = New System.Windows.Forms.Label()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.UserGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'UserGroupBox
        '
        Me.UserGroupBox.Controls.Add(Me.PwTextBox)
        Me.UserGroupBox.Controls.Add(Me.PwLabel)
        Me.UserGroupBox.Controls.Add(Me.Pw2TextBox)
        Me.UserGroupBox.Controls.Add(Me.Pw1TextBox)
        Me.UserGroupBox.Controls.Add(Me.UserTextBox)
        Me.UserGroupBox.Controls.Add(Me.Pw2Label)
        Me.UserGroupBox.Controls.Add(Me.Pw1Label)
        Me.UserGroupBox.Controls.Add(Me.UserLabel)
        Me.UserGroupBox.Location = New System.Drawing.Point(15, 22)
        Me.UserGroupBox.Name = "UserGroupBox"
        Me.UserGroupBox.Size = New System.Drawing.Size(326, 143)
        Me.UserGroupBox.TabIndex = 0
        Me.UserGroupBox.TabStop = False
        Me.UserGroupBox.Text = "Información de usuario"
        '
        'PwTextBox
        '
        Me.PwTextBox.Location = New System.Drawing.Point(128, 60)
        Me.PwTextBox.Name = "PwTextBox"
        Me.PwTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(8226)
        Me.PwTextBox.Size = New System.Drawing.Size(192, 20)
        Me.PwTextBox.TabIndex = 4
        '
        'PwLabel
        '
        Me.PwLabel.AutoSize = True
        Me.PwLabel.Location = New System.Drawing.Point(30, 60)
        Me.PwLabel.Name = "PwLabel"
        Me.PwLabel.Size = New System.Drawing.Size(88, 13)
        Me.PwLabel.TabIndex = 3
        Me.PwLabel.Text = "Password actual:"
        '
        'Pw2TextBox
        '
        Me.Pw2TextBox.Location = New System.Drawing.Point(128, 112)
        Me.Pw2TextBox.Name = "Pw2TextBox"
        Me.Pw2TextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(8226)
        Me.Pw2TextBox.Size = New System.Drawing.Size(192, 20)
        Me.Pw2TextBox.TabIndex = 8
        '
        'Pw1TextBox
        '
        Me.Pw1TextBox.Location = New System.Drawing.Point(128, 86)
        Me.Pw1TextBox.Name = "Pw1TextBox"
        Me.Pw1TextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(8226)
        Me.Pw1TextBox.Size = New System.Drawing.Size(192, 20)
        Me.Pw1TextBox.TabIndex = 6
        '
        'UserTextBox
        '
        Me.UserTextBox.Location = New System.Drawing.Point(128, 30)
        Me.UserTextBox.Name = "UserTextBox"
        Me.UserTextBox.Size = New System.Drawing.Size(192, 20)
        Me.UserTextBox.TabIndex = 2
        '
        'Pw2Label
        '
        Me.Pw2Label.AutoSize = True
        Me.Pw2Label.Location = New System.Drawing.Point(16, 115)
        Me.Pw2Label.Name = "Pw2Label"
        Me.Pw2Label.Size = New System.Drawing.Size(102, 13)
        Me.Pw2Label.TabIndex = 7
        Me.Pw2Label.Text = "Confirmar password:"
        '
        'Pw1Label
        '
        Me.Pw1Label.AutoSize = True
        Me.Pw1Label.Location = New System.Drawing.Point(29, 89)
        Me.Pw1Label.Name = "Pw1Label"
        Me.Pw1Label.Size = New System.Drawing.Size(89, 13)
        Me.Pw1Label.TabIndex = 5
        Me.Pw1Label.Text = "Password nuevo:"
        '
        'UserLabel
        '
        Me.UserLabel.AutoSize = True
        Me.UserLabel.Location = New System.Drawing.Point(72, 33)
        Me.UserLabel.Name = "UserLabel"
        Me.UserLabel.Size = New System.Drawing.Size(46, 13)
        Me.UserLabel.TabIndex = 1
        Me.UserLabel.Text = "Usuario:"
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(228, 171)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(113, 26)
        Me.SaveButton.TabIndex = 16
        Me.SaveButton.Text = "Guardar"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'UserConfForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(353, 205)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.UserGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "UserConfForm"
        Me.Text = "Configuración de usuario"
        Me.UserGroupBox.ResumeLayout(False)
        Me.UserGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UserGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents Pw2Label As System.Windows.Forms.Label
    Friend WithEvents Pw1Label As System.Windows.Forms.Label
    Friend WithEvents UserLabel As System.Windows.Forms.Label
    Friend WithEvents Pw2TextBox As System.Windows.Forms.TextBox
    Friend WithEvents Pw1TextBox As System.Windows.Forms.TextBox
    Friend WithEvents UserTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PwTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PwLabel As System.Windows.Forms.Label
    Friend WithEvents SaveButton As System.Windows.Forms.Button
End Class
