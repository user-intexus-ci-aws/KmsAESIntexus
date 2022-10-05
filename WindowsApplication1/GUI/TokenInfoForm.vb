Public Class TokenInfoForm
    Private tk As Token
    Public Sub New(tk As Token)
        MyBase.New()
        InitializeComponent()
        Me.tk = tk
    End Sub

    Private Sub SendTokenPassphraseButton_Click(sender As Object, e As EventArgs) Handles SendTokenPassphraseButton.Click
        If TokenPassphraseTextBox.Text = "" Then
            MsgBox("Password del token no puede ser vacío")
            Return
        End If

        tk.setPassPhrase(TokenPassphraseTextBox.Text)
        tk.setPassPhraseIsSet(True)
        Me.Dispose()
    End Sub

    Private Sub CancelTokenPassphraseButton_Click(sender As Object, e As EventArgs) Handles CancelTokenPassphraseButton.Click
        tk.setPassPhraseIsSet(False)
        Me.Dispose()
    End Sub
End Class