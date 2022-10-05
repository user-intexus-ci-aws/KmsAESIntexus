Public Class Token
    Private PassPhrase As String
    Private PassPhraseIsSet As Boolean

    Public Sub New()
        PassPhrase = ""
        PassPhraseIsSet = False
    End Sub

    Public Function getPassPhrase() As String
        Return Me.PassPhrase
    End Function

    Public Sub setPassPhrase(PassPhrase As String)
        Me.PassPhrase = PassPhrase
    End Sub

    Public Sub setPassPhraseIsSet(b As Boolean)
        Me.PassPhraseIsSet = b
    End Sub

    Public Function getPassPhraseIsSet() As Boolean
        Return Me.PassPhraseIsSet
    End Function

End Class
