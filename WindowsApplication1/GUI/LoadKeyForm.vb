Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO

Public Class LoadKeyForm
    Private C1 As Component
    Private C2 As Component
    Private C3 As Component
    Private tk As Token
    Private LoadingKey As Boolean

    <DllImport("IntecsusKMSNShield.dll",
            BestFitMapping:=True,
            PreserveSig:=True,
            EntryPoint:="LOAD_ZMK", SetLastError:=False,
            CharSet:=CharSet.Ansi, ExactSpelling:=False,
            CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function LOAD_ZMK(ByVal C1 As String, ByVal C2 As String, ByVal C3 As String, ByVal tokenname As String, ByVal passphrase As String, ByVal zmklabel As String) As Integer
    End Function

    <DllImport("IntecsusKMSNShield.dll",
            BestFitMapping:=True,
            PreserveSig:=True,
            EntryPoint:="KCV_ZMK", SetLastError:=False,
            CharSet:=CharSet.Ansi, ExactSpelling:=False,
            CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function KCV_ZMK(ByVal tokenname As String, ByVal passphrase As String, ByVal zmklabel As String, ByVal KCV As StringBuilder) As Integer
    End Function

    Public Sub New()
        MyBase.New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        C1 = New Component()
        C2 = New Component()
        C3 = New Component()
        tk = New Token()
        LoadingKey = False
    End Sub

    Private Sub MainAppForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        End
    End Sub

    Private Sub C1Button_Click(sender As Object, e As EventArgs) Handles C1Button.Click
        Dim LoadC1Form As LoadComponentForm
        LoadC1Form = New LoadComponentForm("Cargar Componente 1", C1)

        LoadC1Form.StartPosition = FormStartPosition.CenterParent
        LoadC1Form.ShowDialog(Me)
    End Sub

    Private Sub C2Button_Click(sender As Object, e As EventArgs) Handles C2Button.Click
        Dim LoadC2Form As LoadComponentForm
        LoadC2Form = New LoadComponentForm("Cargar Componente 2", C2)

        LoadC2Form.StartPosition = FormStartPosition.CenterParent
        LoadC2Form.ShowDialog(Me)
    End Sub

    Private Sub C3Button_Click(sender As Object, e As EventArgs) Handles C3Button.Click
        Dim LoadC3Form As LoadComponentForm
        LoadC3Form = New LoadComponentForm("Cargar Componente 3", C3)

        LoadC3Form.StartPosition = FormStartPosition.CenterParent
        LoadC3Form.ShowDialog(Me)
    End Sub

    Private Sub LoadKeyForm_Activated(sender As Object, e As EventArgs) Handles Me.Activated

        If C1.getIsValid() And Not C2.getIsValid() And Not C3.getIsValid() Then
            C1Button.Enabled = False
            C2Button.Enabled = True
            C3Button.Enabled = False
            LoadKeyButton.Enabled = False

            C2Button.Select()
        End If

        If C1.getIsValid() And C2.getIsValid() And Not C3.getIsValid() Then
            C1Button.Enabled = False
            C2Button.Enabled = False
            C3Button.Enabled = True
            LoadKeyButton.Enabled = False

            C3Button.Select()
        End If

        If C1.getIsValid() And C2.getIsValid() And C3.getIsValid() And Not LoadingKey Then
            C1Button.Enabled = False
            C2Button.Enabled = False
            C3Button.Enabled = False
            LoadKeyButton.Enabled = True

            LoadKeyButton.Select()
        End If
    End Sub

    Private Sub LoadKeyButton_Click(sender As Object, e As EventArgs) Handles LoadKeyButton.Click
        Dim tkGUI As TokenInfoForm
        Dim res As Integer
        Dim buffer_sb As StringBuilder
        buffer_sb = New StringBuilder(512)
        Dim token_name As String
        Dim key_name As String
        Dim kcv As StringBuilder
        kcv = New StringBuilder("", 512)
        Dim max_reintentos As Integer = 2
        Dim intento As Integer = 0


        Dim length As Long
        FileOpen(1, "C:\test.txt", OpenMode.Input) ' Open file.
        length = LOF(1)   ' Get length of file.
        MsgBox(length)
        FileClose(1)

        token_name = PKCS11Config.getConfig("key", "token_name")
        key_name = PKCS11Config.getConfig("key", "key_name")

        For intento = 0 To max_reintentos

            tkGUI = New TokenInfoForm(Me.tk)
            tkGUI.StartPosition = FormStartPosition.CenterParent
            tkGUI.ShowDialog(Me)

            If Not Me.tk.getPassPhraseIsSet Then
                MsgBox("Proceso cancelado por el usuario")
                GoTo End_Sub
            End If

            LoadingKey = True
            LoadKeyButton.Enabled = False
            Me.Cursor = Cursors.WaitCursor

            res = LOAD_ZMK(
                C1.getComponent(),
                C2.getComponent(),
                C3.getComponent(),
                token_name,
                tk.getPassPhrase(),
                key_name)

            If res <> 0 Then
                Dim Description As String
                Description = PKCS11Error.getErrorDescription(res)
                MsgBox("Ocurrió un error. Descripción: " & Description)

                If res = &HA0 And ((intento + 1) <= max_reintentos) Then
                    MsgBox("Se reintenta operación")
                    Continue For
                ElseIf res = &HA0 And ((intento + 1) > max_reintentos) Then
                    MsgBox("Se alcanzó máximo de reintentos")
                End If
                GoTo End_Sub
            End If

            Exit For

        Next

        res = KCV_ZMK(
            token_name,
            tk.getPassPhrase(),
            key_name,
            kcv)

        If res = 0 Then
            MsgBox("Exito en la carga de llave, KCV: " & UCase(Mid(kcv.ToString(), 1, 6)))
        Else
            Dim Description As String
            Description = PKCS11Error.getErrorDescription(res)
            MsgBox("Ocurrió un error. Descripción: " & Description)
            GoTo End_Sub
        End If

End_Sub:
        LoadingKey = False
        Me.Cursor = Cursors.Arrow
        Call ResetStatus()
        C1.setIsValid(False)
        C2.setIsValid(False)
        C3.setIsValid(False)

    End Sub

    Private Sub ResetStatus()
        Me.C1Button.Enabled = True
        Me.C2Button.Enabled = False
        Me.C3Button.Enabled = False
        Me.LoadKeyButton.Enabled = False
    End Sub
End Class