Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO

Public Class GenKeyForm
    Private C1 As Component
    Private C2 As Component
    Private C3 As Component
    Private tk As Token

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        C1 = New Component()
        C2 = New Component()
        C3 = New Component()
        C1.setIsValid(False)
        C2.setIsValid(False)
        C3.setIsValid(False)
        tk = New Token()

    End Sub
    Private Sub GenKeyForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    <DllImport("IntecsusKMSNShield.dll",
            BestFitMapping:=True,
            PreserveSig:=True,
            EntryPoint:="GENERATE_KEY", SetLastError:=False,
            CharSet:=CharSet.Ansi, ExactSpelling:=False,
            CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function GENERATE_KEY(ByVal C1 As StringBuilder, ByVal C2 As StringBuilder, ByVal C3 As StringBuilder, ByVal tokenname As String, ByVal passphrase As String) As Integer
    End Function

    Private Sub GenKeyButton_Click(sender As Object, e As EventArgs) Handles GenKeyButton.Click
        Dim tkGUI As TokenInfoForm
        Dim res As Integer
        Dim buffer_sb As StringBuilder
        Dim token_name As String
        Dim key_name As String
        Dim kcv As StringBuilder
        Dim LocalC1, LocalC2, LocalC3 As StringBuilder
        Dim keyKCV As String
        Dim keyLeftKCV As String
        Dim keyRightKCV As String
        Dim hu As HashUtils = New HashUtils()
        Dim key As String
        Dim keyLeft As String
        Dim keyRight As String

        kcv = New StringBuilder("", 512)
        buffer_sb = New StringBuilder(512)
        LocalC1 = New StringBuilder(128)
        LocalC2 = New StringBuilder(128)
        LocalC3 = New StringBuilder(128)

        token_name = PKCS11Config.getConfig("key", "token_name")
        key_name = PKCS11Config.getConfig("key", "key_name")


        tkGUI = New TokenInfoForm(Me.tk)
        tkGUI.StartPosition = FormStartPosition.CenterParent
        tkGUI.ShowDialog(Me)

        If Not Me.tk.getPassPhraseIsSet Then
            MsgBox("Proceso cancelado por el usuario")
            GoTo End_Sub
        End If

        Me.Cursor = Cursors.WaitCursor

        res = GENERATE_KEY(
            LocalC1,
            LocalC2,
            LocalC3,
            token_name,
            tk.getPassPhrase())

        If res <> 0 Then
            Dim Description As String
            Description = PKCS11Error.getErrorDescription(res)
            MsgBox("Ocurrió un error. Descripción: " & Description)
            GoTo End_Sub
        End If

        C1.setComponent(UCase(LocalC1.ToString()))
        C2.setComponent(UCase(LocalC2.ToString()))
        C3.setComponent(UCase(LocalC3.ToString()))

        Dim tdes As TDES
        tdes = New TDES()

        C1.setKCV(UCase(tdes.DigitoChequeo(6, LocalC1.ToString())))
        C2.setKCV(UCase(tdes.DigitoChequeo(6, LocalC2.ToString())))
        C3.setKCV(UCase(tdes.DigitoChequeo(6, LocalC3.ToString())))

        key = hu.XorHexString(LocalC1.ToString, LocalC2.ToString)
        key = hu.XorHexString(key, LocalC3.ToString)

        keyLeft = key.Substring(0, 16) & key.Substring(0, 16)
        keyRight = key.Substring(16, 16) & key.Substring(16, 16)

        keyKCV = UCase(tdes.DigitoChequeo(6, key))
        keyLeftKCV = UCase(tdes.DigitoChequeo(6, keyLeft))
        keyRightKCV = UCase(tdes.DigitoChequeo(6, keyRight))

        C1.setKeyKCV(keyKCV)
        C1.setLeftKeyKCV(keyLeftKCV)
        C1.setRightKeyKCV(keyRightKCV)

        C2.setKeyKCV(keyKCV)
        C2.setLeftKeyKCV(keyLeftKCV)
        C2.setRightKeyKCV(keyRightKCV)

        C3.setKeyKCV(keyKCV)
        C3.setLeftKeyKCV(keyLeftKCV)
        C3.setRightKeyKCV(keyRightKCV)

        ShowC1Button.Enabled = True
        GenKeyButton.Enabled = False

End_Sub:
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ShowC1Button.Click
        Dim ShowC1Form As ShowComponentForm
        ShowC1Form = New ShowComponentForm("Mostrar Componente 1", C1)

        ShowC1Form.StartPosition = FormStartPosition.CenterParent
        ShowC1Form.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ShowC2Button.Click
        Dim ShowC2Form As ShowComponentForm
        ShowC2Form = New ShowComponentForm("Mostrar Componente 2", C2)

        ShowC2Form.StartPosition = FormStartPosition.CenterParent
        ShowC2Form.ShowDialog(Me)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles ShowC3Button.Click
        Dim ShowC3Form As ShowComponentForm
        ShowC3Form = New ShowComponentForm("Mostrar Componente 3", C3)

        ShowC3Form.StartPosition = FormStartPosition.CenterParent
        ShowC3Form.ShowDialog(Me)
    End Sub

    Private Sub GenKeyForm_Activated(sender As Object, e As EventArgs) Handles Me.Activated

        If C1.getIsValid() And Not C2.getIsValid() And Not C3.getIsValid() Then
            ShowC1Button.Enabled = False
            ShowC2Button.Enabled = True
            ShowC3Button.Enabled = False
            GenKeyButton.Enabled = False

            ShowC2Button.Select()
        End If

        If C1.getIsValid() And C2.getIsValid() And Not C3.getIsValid() Then
            ShowC1Button.Enabled = False
            ShowC2Button.Enabled = False
            ShowC3Button.Enabled = True
            GenKeyButton.Enabled = False

            ShowC3Button.Select()
        End If

        If C1.getIsValid() And C2.getIsValid() And C3.getIsValid() Then
            C1.setIsValid(False)
            C2.setIsValid(False)
            C3.setIsValid(False)

            ShowC1Button.Enabled = False
            ShowC2Button.Enabled = False
            ShowC3Button.Enabled = False
            GenKeyButton.Enabled = True

            GenKeyButton.Select()
        End If
    End Sub

End Class