<Serializable()>
Public Class Key
    Private Key As String

    Public lstComponents As List(Of Component)

    Function getKey() As String
        Return Me.Key
    End Function

    Sub setKey(Key As String)
        Me.Key = Key
    End Sub
End Class
