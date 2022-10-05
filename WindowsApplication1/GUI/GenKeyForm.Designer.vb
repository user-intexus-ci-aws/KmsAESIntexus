<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GenKeyForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GenKeyForm))
        Me.GenKeyGroupBox = New System.Windows.Forms.GroupBox()
        Me.GenKeyButton = New System.Windows.Forms.Button()
        Me.ShowC3Button = New System.Windows.Forms.Button()
        Me.ShowC2Button = New System.Windows.Forms.Button()
        Me.ShowC1Button = New System.Windows.Forms.Button()
        Me.GenKeyGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'GenKeyGroupBox
        '
        Me.GenKeyGroupBox.Controls.Add(Me.GenKeyButton)
        Me.GenKeyGroupBox.Controls.Add(Me.ShowC3Button)
        Me.GenKeyGroupBox.Controls.Add(Me.ShowC2Button)
        Me.GenKeyGroupBox.Controls.Add(Me.ShowC1Button)
        Me.GenKeyGroupBox.Location = New System.Drawing.Point(10, 11)
        Me.GenKeyGroupBox.Margin = New System.Windows.Forms.Padding(2)
        Me.GenKeyGroupBox.Name = "GenKeyGroupBox"
        Me.GenKeyGroupBox.Padding = New System.Windows.Forms.Padding(2)
        Me.GenKeyGroupBox.Size = New System.Drawing.Size(294, 263)
        Me.GenKeyGroupBox.TabIndex = 0
        Me.GenKeyGroupBox.TabStop = False
        Me.GenKeyGroupBox.Text = "Creación de Llave"
        '
        'GenKeyButton
        '
        Me.GenKeyButton.Location = New System.Drawing.Point(73, 25)
        Me.GenKeyButton.Margin = New System.Windows.Forms.Padding(2)
        Me.GenKeyButton.Name = "GenKeyButton"
        Me.GenKeyButton.Size = New System.Drawing.Size(149, 53)
        Me.GenKeyButton.TabIndex = 0
        Me.GenKeyButton.Text = "Generar Nueva Llave"
        Me.GenKeyButton.UseVisualStyleBackColor = True
        '
        'ShowC3Button
        '
        Me.ShowC3Button.Enabled = False
        Me.ShowC3Button.Location = New System.Drawing.Point(73, 195)
        Me.ShowC3Button.Margin = New System.Windows.Forms.Padding(2)
        Me.ShowC3Button.Name = "ShowC3Button"
        Me.ShowC3Button.Size = New System.Drawing.Size(149, 53)
        Me.ShowC3Button.TabIndex = 3
        Me.ShowC3Button.Text = "Mostrar Componente 3"
        Me.ShowC3Button.UseVisualStyleBackColor = True
        '
        'ShowC2Button
        '
        Me.ShowC2Button.Enabled = False
        Me.ShowC2Button.Location = New System.Drawing.Point(73, 138)
        Me.ShowC2Button.Margin = New System.Windows.Forms.Padding(2)
        Me.ShowC2Button.Name = "ShowC2Button"
        Me.ShowC2Button.Size = New System.Drawing.Size(149, 53)
        Me.ShowC2Button.TabIndex = 2
        Me.ShowC2Button.Text = "Mostrar Componente 2"
        Me.ShowC2Button.UseVisualStyleBackColor = True
        '
        'ShowC1Button
        '
        Me.ShowC1Button.Enabled = False
        Me.ShowC1Button.Location = New System.Drawing.Point(73, 81)
        Me.ShowC1Button.Margin = New System.Windows.Forms.Padding(2)
        Me.ShowC1Button.Name = "ShowC1Button"
        Me.ShowC1Button.Size = New System.Drawing.Size(149, 53)
        Me.ShowC1Button.TabIndex = 1
        Me.ShowC1Button.Text = "Mostrar Componente 1"
        Me.ShowC1Button.UseVisualStyleBackColor = True
        '
        'GenKeyForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(313, 284)
        Me.Controls.Add(Me.GenKeyGroupBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "GenKeyForm"
        Me.Text = "Generar Llave"
        Me.GenKeyGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GenKeyGroupBox As GroupBox
    Friend WithEvents ShowC3Button As Button
    Friend WithEvents ShowC2Button As Button
    Friend WithEvents ShowC1Button As Button
    Friend WithEvents GenKeyButton As Button
End Class
