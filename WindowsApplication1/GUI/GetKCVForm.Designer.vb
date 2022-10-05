<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GetKCVForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GetKCVForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.KeyNameTextBox = New System.Windows.Forms.TextBox()
        Me.KeyNameLabel = New System.Windows.Forms.Label()
        Me.GetKCVButton = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.KeyNameTextBox)
        Me.GroupBox1.Controls.Add(Me.KeyNameLabel)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 23)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(454, 83)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Información de la llave"
        '
        'KeyNameTextBox
        '
        Me.KeyNameTextBox.Location = New System.Drawing.Point(122, 39)
        Me.KeyNameTextBox.Name = "KeyNameTextBox"
        Me.KeyNameTextBox.Size = New System.Drawing.Size(315, 20)
        Me.KeyNameTextBox.TabIndex = 1
        '
        'KeyNameLabel
        '
        Me.KeyNameLabel.AutoSize = True
        Me.KeyNameLabel.Location = New System.Drawing.Point(18, 42)
        Me.KeyNameLabel.Name = "KeyNameLabel"
        Me.KeyNameLabel.Size = New System.Drawing.Size(98, 13)
        Me.KeyNameLabel.TabIndex = 0
        Me.KeyNameLabel.Text = "Nombre de la llave:"
        '
        'GetKCVButton
        '
        Me.GetKCVButton.Enabled = False
        Me.GetKCVButton.Location = New System.Drawing.Point(330, 112)
        Me.GetKCVButton.Name = "GetKCVButton"
        Me.GetKCVButton.Size = New System.Drawing.Size(136, 28)
        Me.GetKCVButton.TabIndex = 1
        Me.GetKCVButton.Text = "Consultar KCV"
        Me.GetKCVButton.UseVisualStyleBackColor = True
        '
        'GetKCVForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(478, 151)
        Me.Controls.Add(Me.GetKCVButton)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "GetKCVForm"
        Me.Text = "Consulta de KCV"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents KeyNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents KeyNameLabel As System.Windows.Forms.Label
    Friend WithEvents GetKCVButton As System.Windows.Forms.Button
End Class
