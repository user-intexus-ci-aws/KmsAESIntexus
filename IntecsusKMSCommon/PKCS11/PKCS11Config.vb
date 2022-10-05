Imports System.IO
Imports System.Text
Imports System.Environment

Public Class PKCS11Config
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

    Public Shared Function getConfig(section As String, name As String) As String
        Dim res As Integer
        Dim buffer_sb As StringBuilder
        buffer_sb = New StringBuilder(512)

        res = GetPrivateProfileString(section, name, "", buffer_sb, buffer_sb.Capacity, GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)
        Return buffer_sb.ToString()
    End Function

    Public Shared Function setConfig(section As String, name As String, value As String) As Integer
        Dim res As Integer
        res = WritePrivateProfileString(section, name, value, GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)
        Return res
    End Function
End Class
