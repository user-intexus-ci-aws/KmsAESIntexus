Imports System.Text
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Environment

Public Class GetKCVForm
    <DllImport("IntecsusKMSNShield.dll",
            BestFitMapping:=True,
            PreserveSig:=True,
            EntryPoint:="KCV_ZMK", SetLastError:=False,
            CharSet:=CharSet.Ansi, ExactSpelling:=False,
            CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function KCV_ZMK(ByVal tokenname As String, ByVal passphrase As String, ByVal zmklabel As String, ByVal KCV As StringBuilder) As Integer
    End Function

    Private Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpSectionName As String, _
                    ByVal lpKeyName As String, _
                    ByVal lpDefault As String, _
                    ByVal lpReturnedString As StringBuilder, _
                    ByVal nSize As Integer, _
                    ByVal lpFileName As String) As Integer

    Private Const CONFIG_PATH As String = "\IntecsusKMS\config.ini"

    Private Sub GetKCVButton_Click(sender As Object, e As EventArgs) Handles GetKCVButton.Click
        Dim kcv As StringBuilder
        kcv = New StringBuilder("", 512)
        Dim res As Integer
        Dim tkGUI As TokenInfoForm
        Dim l_token As Token
        l_token = New Token()
        Dim token_name As String
        Dim buffer_sb As StringBuilder
        buffer_sb = New StringBuilder(512)

        res = GetPrivateProfileString("key", "token_name", "", buffer_sb, buffer_sb.Capacity, GetFolderPath(SpecialFolder.ApplicationData) & CONFIG_PATH)
        token_name = buffer_sb.ToString()

        tkGUI = New TokenInfoForm(l_token)
        tkGUI.StartPosition = FormStartPosition.CenterParent
        tkGUI.ShowDialog(Me)

        If Not l_token.getPassPhraseIsSet Then
            MsgBox("Proceso cancelado")
            GoTo End_Sub
        End If

        res = KCV_ZMK( _
            token_name, _
            l_token.getPassPhrase(), _
            KeyNameTextBox.Text, _
            kcv)

        If res = 0 Then
            MsgBox("Llave: " & KeyNameTextBox.Text & ", KCV: " & UCase(Mid(kcv.ToString(), 1, 6)))
        Else
            Dim Description As String
            Description = PKCS11Error.getErrorDescription(res)
            MsgBox("Ocurrió un error. Descripción: " & Description)
            GoTo End_Sub
        End If

End_Sub:
        Me.Dispose()
    End Sub

    Private Sub KeyNameTextBox_OnChange(sender As Object, e As EventArgs) Handles KeyNameTextBox.TextChanged
        If KeyNameTextBox.Text = "" Then
            GetKCVButton.Enabled = False
        Else
            GetKCVButton.Enabled = True
        End If
    End Sub
End Class