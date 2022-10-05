Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO
Imports System.Security.Cryptography
Imports System.Environment

Public Class LoginForm

    Private Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpAppName As String, _
                ByVal lpKeyName As String, _
                ByVal lpDefault As String, _
                ByVal lpReturnedString As StringBuilder, _
                ByVal nSize As Integer, _
                ByVal lpFileName As String) As Integer

    Private Const CONFIG_PATH As String = "\IntecsusKMS\config.ini"

    Private Sub LoginButton_Click(sender As Object, e As EventArgs) Handles LoginButton.Click

        Dim res As Integer
        Dim user_sb As StringBuilder
        Dim pw_sb As StringBuilder
        Dim hutil As HashUtils

        hutil = New HashUtils()

        user_sb = New StringBuilder(1024)
        pw_sb = New StringBuilder(1024)

        res = GetPrivateProfileString("login", "user", "", user_sb, user_sb.Capacity, GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)
        res = GetPrivateProfileString("login", "pw", "", pw_sb, pw_sb.Capacity, GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)

        If UserTextBox.Text = "" Then
            MsgBox("El usuario no puede ser vacío")
            Return
        End If

        If PasswordTextBox.Text = "" Then
            MsgBox("El password no puede ser vacío")
            Return
        End If

        If String.Compare(UserTextBox.Text, user_sb.ToString(), True) = 0 And _
            String.Compare(hutil.GetSha512(PasswordTextBox.Text), Trim(pw_sb.ToString()), True) = 0 Then
            Me.Enabled = False
            Me.Hide()

            MainAppForm.StartPosition = FormStartPosition.CenterScreen
            MainAppForm.Enabled = True
            MainAppForm.Show(Me)
        Else
            MsgBox("Error al validar usuario", MsgBoxStyle.Exclamation, "Error")
            PasswordTextBox.Text = ""
        End If
    End Sub
End Class