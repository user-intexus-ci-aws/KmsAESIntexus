<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainAppForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainAppForm))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.OpcionesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfiguraciónToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LlavesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfLlaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConsultarKCVToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AyudaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AcercaDeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OperacionesGroupBox = New System.Windows.Forms.GroupBox()
        Me.LoadKeyButton = New System.Windows.Forms.Button()
        Me.GenKeyButton = New System.Windows.Forms.Button()
        Me.MenuStrip1.SuspendLayout()
        Me.OperacionesGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpcionesToolStripMenuItem, Me.LlavesToolStripMenuItem, Me.AyudaToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(346, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'OpcionesToolStripMenuItem
        '
        Me.OpcionesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConfiguraciónToolStripMenuItem})
        Me.OpcionesToolStripMenuItem.Name = "OpcionesToolStripMenuItem"
        Me.OpcionesToolStripMenuItem.Size = New System.Drawing.Size(64, 20)
        Me.OpcionesToolStripMenuItem.Text = "Usuarios"
        '
        'ConfiguraciónToolStripMenuItem
        '
        Me.ConfiguraciónToolStripMenuItem.Name = "ConfiguraciónToolStripMenuItem"
        Me.ConfiguraciónToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ConfiguraciónToolStripMenuItem.Text = "Configuración de usuario"
        '
        'LlavesToolStripMenuItem
        '
        Me.LlavesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConfLlaveToolStripMenuItem, Me.ConsultarKCVToolStripMenuItem})
        Me.LlavesToolStripMenuItem.Name = "LlavesToolStripMenuItem"
        Me.LlavesToolStripMenuItem.Size = New System.Drawing.Size(51, 20)
        Me.LlavesToolStripMenuItem.Text = "Llaves"
        '
        'ConfLlaveToolStripMenuItem
        '
        Me.ConfLlaveToolStripMenuItem.Name = "ConfLlaveToolStripMenuItem"
        Me.ConfLlaveToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.ConfLlaveToolStripMenuItem.Text = "Configuración de llave"
        '
        'ConsultarKCVToolStripMenuItem
        '
        Me.ConsultarKCVToolStripMenuItem.Name = "ConsultarKCVToolStripMenuItem"
        Me.ConsultarKCVToolStripMenuItem.Size = New System.Drawing.Size(193, 22)
        Me.ConsultarKCVToolStripMenuItem.Text = "Consultar de KCV"
        '
        'AyudaToolStripMenuItem
        '
        Me.AyudaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AcercaDeToolStripMenuItem})
        Me.AyudaToolStripMenuItem.Name = "AyudaToolStripMenuItem"
        Me.AyudaToolStripMenuItem.Size = New System.Drawing.Size(53, 20)
        Me.AyudaToolStripMenuItem.Text = "Ayuda"
        '
        'AcercaDeToolStripMenuItem
        '
        Me.AcercaDeToolStripMenuItem.Name = "AcercaDeToolStripMenuItem"
        Me.AcercaDeToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.AcercaDeToolStripMenuItem.Text = "Acerca de ..."
        '
        'OperacionesGroupBox
        '
        Me.OperacionesGroupBox.Controls.Add(Me.LoadKeyButton)
        Me.OperacionesGroupBox.Controls.Add(Me.GenKeyButton)
        Me.OperacionesGroupBox.Location = New System.Drawing.Point(9, 25)
        Me.OperacionesGroupBox.Margin = New System.Windows.Forms.Padding(2)
        Me.OperacionesGroupBox.Name = "OperacionesGroupBox"
        Me.OperacionesGroupBox.Padding = New System.Windows.Forms.Padding(2)
        Me.OperacionesGroupBox.Size = New System.Drawing.Size(328, 232)
        Me.OperacionesGroupBox.TabIndex = 2
        Me.OperacionesGroupBox.TabStop = False
        Me.OperacionesGroupBox.Text = "Operaciones"
        '
        'LoadKeyButton
        '
        Me.LoadKeyButton.Location = New System.Drawing.Point(56, 127)
        Me.LoadKeyButton.Margin = New System.Windows.Forms.Padding(2)
        Me.LoadKeyButton.Name = "LoadKeyButton"
        Me.LoadKeyButton.Size = New System.Drawing.Size(218, 65)
        Me.LoadKeyButton.TabIndex = 1
        Me.LoadKeyButton.Text = "Cargar Llave"
        Me.LoadKeyButton.UseVisualStyleBackColor = True
        '
        'GenKeyButton
        '
        Me.GenKeyButton.Location = New System.Drawing.Point(56, 44)
        Me.GenKeyButton.Margin = New System.Windows.Forms.Padding(2)
        Me.GenKeyButton.Name = "GenKeyButton"
        Me.GenKeyButton.Size = New System.Drawing.Size(218, 64)
        Me.GenKeyButton.TabIndex = 0
        Me.GenKeyButton.Text = "Generar Llave"
        Me.GenKeyButton.UseVisualStyleBackColor = True
        '
        'MainAppForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(346, 278)
        Me.Controls.Add(Me.OperacionesGroupBox)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "MainAppForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "IntecsusKMS"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.OperacionesGroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents OpcionesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AyudaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AcercaDeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConfiguraciónToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LlavesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConsultarKCVToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OperacionesGroupBox As GroupBox
    Friend WithEvents LoadKeyButton As Button
    Friend WithEvents GenKeyButton As Button
    Friend WithEvents ConfLlaveToolStripMenuItem As ToolStripMenuItem
End Class
