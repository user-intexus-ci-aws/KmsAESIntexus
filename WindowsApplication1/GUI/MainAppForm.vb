Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO

Public Class MainAppForm
    Public Sub New()
        MyBase.New()
        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    Private Sub MainAppForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        End
    End Sub

    Private Sub AcercaDeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AcercaDeToolStripMenuItem.Click
        Dim AboutForm As AboutForm
        AboutForm = New AboutForm()
        AboutForm.StartPosition = FormStartPosition.CenterParent
        AboutForm.ShowDialog(Me)
    End Sub

    Private Sub ConfLlaveToolStripMenuItem_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub ConsultarKCVToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConsultarKCVToolStripMenuItem.Click
        Dim GetKCVForm As GetKCVForm
        GetKCVForm = New GetKCVForm()
        GetKCVForm.StartPosition = FormStartPosition.CenterParent
        GetKCVForm.ShowDialog(Me)
    End Sub

    Private Sub LoadKeyButton_Click(sender As Object, e As EventArgs) Handles LoadKeyButton.Click
        Dim LoadKeyForm As LoadKeyForm
        LoadKeyForm = New LoadKeyForm()

        LoadKeyForm.StartPosition = FormStartPosition.CenterParent
        LoadKeyForm.ShowDialog(Me)
    End Sub

    Private Sub GenKeyButton_Click(sender As Object, e As EventArgs) Handles GenKeyButton.Click
        Dim GenKeyForm As GenKeyForm
        GenKeyForm = New GenKeyForm()

        GenKeyForm.StartPosition = FormStartPosition.CenterParent
        GenKeyForm.ShowDialog(Me)

    End Sub

    Private Sub ConfLlaveToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles ConfLlaveToolStripMenuItem.Click
        Dim KeyConfForm As KeyConfForm
        KeyConfForm = New KeyConfForm()
        KeyConfForm.StartPosition = FormStartPosition.CenterParent
        KeyConfForm.ShowDialog(Me)
    End Sub

    Private Sub ConfiguraciónToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfiguraciónToolStripMenuItem.Click
        Dim UserConfForm As UserConfForm
        UserConfForm = New UserConfForm()
        UserConfForm.StartPosition = FormStartPosition.CenterParent
        UserConfForm.ShowDialog(Me)
    End Sub
End Class