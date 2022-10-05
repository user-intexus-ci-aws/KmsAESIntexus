Public Class Component
    Private Component As String
    Private KCV As String
    Private KeyKCV As String
    Private LeftKeyKCV As String
    Private RightKeyKCV As String
    Private isValid As Boolean

    Public Sub New()
        Me.isValid = False
    End Sub

    Sub setComponent(Component As String)
        Me.Component = Component
    End Sub

    Sub setKCV(KCV As String)
        Me.KCV = KCV
    End Sub

    Sub setKeyKCV(KCV As String)
        Me.KeyKCV = KCV
    End Sub

    Sub setLeftKeyKCV(KCV As String)
        Me.LeftKeyKCV = KCV
    End Sub

    Sub setRightKeyKCV(KCV As String)
        Me.RightKeyKCV = KCV
    End Sub

    Sub setIsValid(isValid As Boolean)
        Me.isValid = isValid
    End Sub

    Function getComponent() As String
        Return Me.Component
    End Function

    Function getKCV() As String
        Return Me.KCV
    End Function

    Function getKeyKCV() As String
        Return Me.KeyKCV
    End Function

    Function getLeftKeyKCV() As String
        Return Me.LeftKeyKCV
    End Function

    Function getRightKeyKCV() As String
        Return Me.RightKeyKCV
    End Function

    Function getIsValid() As Boolean
        Return Me.isValid
    End Function

End Class
