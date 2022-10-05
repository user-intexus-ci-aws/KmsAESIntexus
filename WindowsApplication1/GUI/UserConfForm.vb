Imports System.Text
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Environment

Public Class UserConfForm

    Private Shared SpecialChars As String = "¡!#$%&/()=¿?"
    Private Const CONFIG_PATH As String = "\IntecsusKMS\config.ini"

    Private Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpSectionName As String, _
        ByVal lpKeyName As String, _
        ByVal lpDefault As String, _
        ByVal lpReturnedString As StringBuilder, _
        ByVal nSize As Integer, _
        ByVal lpFileName As String) As Integer

    Private Declare Auto Function WritePrivateProfileString Lib "kernel32" (ByVal lpSectionName As String, _
       ByVal lpKeyName As String, _
       ByVal lpString As String, _
       ByVal lpFileName As String) As Integer

    Public Sub New()
        MyBase.New()
        InitializeComponent()

        LoadValues()
    End Sub

    Private Sub LoadValues()
        Dim res As Integer
        Dim buffer_sb As StringBuilder
        buffer_sb = New StringBuilder(512)

        res = GetPrivateProfileString("login", "user", "", buffer_sb, buffer_sb.Capacity, GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)
        UserTextBox.Text = buffer_sb.ToString()
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        Dim hutils As HashUtils
        Dim res As Integer
        Dim pw_sb As StringBuilder

        hutils = New HashUtils()
        pw_sb = New StringBuilder(1024)

        If UserTextBox.Text = "" Then
            MsgBox("Usuario no puede ser vacío")
            LoadValues()
            Return
        End If

        If PwTextBox.Text = "" Or Pw1TextBox.Text = "" Or Pw2TextBox.Text = "" Then
            MsgBox("Ninguno de los passwords pueden ser vacíos")
            LoadValues()
            Return
        End If

        res = GetPrivateProfileString("login", "pw", "", pw_sb, pw_sb.Capacity, GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)

        If String.Compare(pw_sb.ToString(), hutils.GetSha512(PwTextBox.Text), True) <> 0 Then
            MsgBox("Error al validar usuario actual", MsgBoxStyle.Exclamation, "Error")
        Else
            If String.Compare(Pw1TextBox.Text, Pw2TextBox.Text, False) <> 0 Then
                MsgBox("Nuevo password no coincide con confirmación", MsgBoxStyle.Exclamation, "Error")
            Else
                If Not IsStrongPassword(Pw1TextBox.Text) Then
                    MsgBox("Password nuevo debe: tener longitud >= 8, tener al menos: una mayúscula, una minúscula, un digito y un caracter especial: " + SpecialChars)
                    Return
                End If

                res = WritePrivateProfileString("login", "user", UserTextBox.Text, GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)
                res = WritePrivateProfileString("login", "pw", hutils.GetSha512(Pw1TextBox.Text), GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)

                MsgBox("Usuario actualizado")

                Me.Dispose()
            End If
        End If
    End Sub

    Private Function IsStrongPassword(password As String) As Boolean
        Dim upperCount As Integer
        Dim lowerCount As Integer
        Dim digitCount As Integer
        Dim symbolCount As Integer

        upperCount = 0
        lowerCount = 0
        digitCount = 0
        symbolCount = 0

        For i As Integer = 1 To password.Length
            If Char.IsUpper(GetChar(password, i)) Then
                upperCount += 1
            ElseIf Char.IsLower(GetChar(password, i)) Then
                lowerCount += 1
            ElseIf Char.IsDigit(GetChar(password, i)) Then
                digitCount += 1
            ElseIf SpecialChars.IndexOf(GetChar(password, i)) >= 0 Then
                symbolCount += 1
            End If
        Next

        'MsgBox(upperCount & ", " & lowerCount & ", " & digitCount & ", " & symbolCount)

        Return password.Length >= 8 And upperCount >= 1 And lowerCount >= 1 And digitCount >= 1 And symbolCount >= 1

    End Function
End Class