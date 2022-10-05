Imports System.Text.RegularExpressions

Public Class LoadComponentForm
    Private Component As Component

    Public Sub New(Title As String, Component As Component)
        MyBase.New()
        InitializeComponent()
        Me.Text = Title
        Me.Component = Component
        LoadComponentButton.Enabled = False
    End Sub

    Private Sub LoadComponentButton_Click(sender As Object, e As EventArgs) Handles LoadComponentButton.Click
        Me.Component.setIsValid(True)
        Me.Component.setComponent(ComponentTextBox.Text)

        MainAppForm.Enabled = True
        Me.Dispose()
    End Sub

    Private Sub ComponentTextBox_OnChange(sender As Object, e As EventArgs) Handles ComponentTextBox.TextChanged
        If Not Regex.Match(ComponentTextBox.Text, "^[0-9,A-F]*$", RegexOptions.IgnoreCase).Success Then
            FormatLabel.Text = "Usar sólo caracteres hexadecimal: 0-9, A-F"
        Else
            FormatLabel.Text = ""
            If Len(ComponentTextBox.Text) = 32 Then
                Dim tdes As TDES
                tdes = New TDES()
                KCVTextBox.Text = UCase(tdes.DigitoChequeo(6, ComponentTextBox.Text))
                LoadComponentButton.Enabled = True
            Else
                KCVTextBox.Text = ""
                LoadComponentButton.Enabled = False
            End If
        End If
    End Sub

    Private Sub LoadComponentForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        MainAppForm.Enabled = True
    End Sub

    Private Sub KCVTextBox_TextChanged(sender As Object, e As EventArgs) Handles KCVTextBox.TextChanged

    End Sub

    Private Sub LoadComponentForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class