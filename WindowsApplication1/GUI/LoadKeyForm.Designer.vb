<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoadKeyForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LoadKeyForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.C3Button = New System.Windows.Forms.Button()
        Me.LoadKeyButton = New System.Windows.Forms.Button()
        Me.C2Button = New System.Windows.Forms.Button()
        Me.C1Button = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.C3Button)
        Me.GroupBox1.Controls.Add(Me.LoadKeyButton)
        Me.GroupBox1.Controls.Add(Me.C2Button)
        Me.GroupBox1.Controls.Add(Me.C1Button)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 11)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(320, 271)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Carga de Llave"
        '
        'C3Button
        '
        Me.C3Button.Enabled = False
        Me.C3Button.Location = New System.Drawing.Point(88, 141)
        Me.C3Button.Name = "C3Button"
        Me.C3Button.Size = New System.Drawing.Size(149, 53)
        Me.C3Button.TabIndex = 2
        Me.C3Button.Text = "Cargar Componente 3"
        Me.C3Button.UseVisualStyleBackColor = True
        '
        'LoadKeyButton
        '
        Me.LoadKeyButton.Enabled = False
        Me.LoadKeyButton.Location = New System.Drawing.Point(88, 201)
        Me.LoadKeyButton.Name = "LoadKeyButton"
        Me.LoadKeyButton.Size = New System.Drawing.Size(149, 53)
        Me.LoadKeyButton.TabIndex = 3
        Me.LoadKeyButton.Text = "Cargar Llave en HSM"
        Me.LoadKeyButton.UseVisualStyleBackColor = True
        '
        'C2Button
        '
        Me.C2Button.Enabled = False
        Me.C2Button.Location = New System.Drawing.Point(88, 82)
        Me.C2Button.Name = "C2Button"
        Me.C2Button.Size = New System.Drawing.Size(149, 53)
        Me.C2Button.TabIndex = 1
        Me.C2Button.Text = "Cargar Componente 2"
        Me.C2Button.UseVisualStyleBackColor = True
        '
        'C1Button
        '
        Me.C1Button.Location = New System.Drawing.Point(88, 23)
        Me.C1Button.Name = "C1Button"
        Me.C1Button.Size = New System.Drawing.Size(149, 53)
        Me.C1Button.TabIndex = 0
        Me.C1Button.Text = "Cargar Componente 1"
        Me.C1Button.UseVisualStyleBackColor = True
        '
        'LoadKeyForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(340, 293)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "LoadKeyForm"
        Me.Text = "Cargar Llave"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents C3Button As Button
    Friend WithEvents LoadKeyButton As Button
    Friend WithEvents C2Button As Button
    Friend WithEvents C1Button As Button
End Class
