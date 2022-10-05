Imports System.Text
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Environment

Public Class KeyConfForm

    Private Const CONFIG_PATH As String = "\IntecsusKMS\config.ini"

    Private Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpSectionName As String,
        ByVal lpKeyName As String,
        ByVal lpDefault As String,
        ByVal lpReturnedString As StringBuilder,
        ByVal nSize As Integer,
        ByVal lpFileName As String) As Integer

    Private Declare Auto Function WritePrivateProfileString Lib "kernel32" (ByVal lpSectionName As String,
       ByVal lpKeyName As String,
       ByVal lpString As String,
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

        res = GetPrivateProfileString("key", "key_name", "", buffer_sb, buffer_sb.Capacity, GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)
        KeyNameTextBox.Text = buffer_sb.ToString()
        res = GetPrivateProfileString("key", "token_name", "", buffer_sb, buffer_sb.Capacity, GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)
        TokenNameTextBox.Text = buffer_sb.ToString()
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        Dim res As Integer

        If KeyNameTextBox.Text = "" Then
            MsgBox("Nombre de la llave no puede ser vacío")
            LoadValues()
            Return
        End If

        If TokenNameTextBox.Text = "" Then
            MsgBox("Nombre del token no puede ser vacío")
            LoadValues()
            Return
        End If

        res = WritePrivateProfileString("key", "key_name", KeyNameTextBox.Text, GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)
        res = WritePrivateProfileString("key", "token_name", TokenNameTextBox.Text, GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)
        MsgBox("Nombre de llave y token actualizados")

        Me.Dispose()
    End Sub

End Class