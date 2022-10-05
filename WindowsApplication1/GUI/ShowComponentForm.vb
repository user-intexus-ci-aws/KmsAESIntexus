Imports System.Text.RegularExpressions

Public Class ShowComponentForm
    Private Condition1 As Boolean
    Private Condition2 As Boolean
    Private Condition3 As Boolean
    Private Condition4 As Boolean
    Private Condition5 As Boolean
    Private C As Component

    Public Sub New(Title As String, Component As Component)
        MyBase.New()
        InitializeComponent()
        Me.C = Component
        Me.Text = Title
        Me.OriginalComponentTextBox.Text = Component.getComponent()
        Me.OriginalComponentKCVTextBox.Text = Component.getKCV()
        Me.OriginalKeyKCVTextBox.Text = Component.getKeyKCV()
        Me.OriginalKeyLeftKCVTextBox.Text = Component.getLeftKeyKCV()
        Me.OriginalKeyRightKCVTextBox.Text = Component.getRightKeyKCV()
        Me.EndShowComponentButton.Enabled = False
        Condition1 = Condition2 = Condition3 = Condition4 = Condition5 = False
    End Sub

    Private Sub ValidateConditions()
        If Condition1 And Condition2 And Condition3 And Condition4 And Condition5 Then
            EndShowComponentButton.Enabled = True
        End If
    End Sub

    Private Sub CopiedComponentTextBox_TextChanged(sender As Object, e As EventArgs) Handles CopiedComponentTextBox.TextChanged
        If Not Regex.Match(CopiedComponentTextBox.Text, "^[0-9,A-F]*$", RegexOptions.IgnoreCase).Success Then
            CopiedComponentFormatLabel.Text = "Usar sólo caracteres hexadecimal: 0-9, A-F"
        Else
            CopiedComponentFormatLabel.Text = ""
            If Len(CopiedComponentTextBox.Text) = 32 Then
                If OriginalComponentTextBox.Text <> CopiedComponentTextBox.Text Then
                    CopiedComponentFormatLabel.Text = "Componente no coincide"
                    EndShowComponentButton.Enabled = False
                    Condition1 = False
                Else
                    Condition1 = True
                    ValidateConditions()
                End If
            Else
                EndShowComponentButton.Enabled = False
                Condition1 = False
            End If
        End If
    End Sub

    Private Sub EndShowComponentButton_Click(sender As Object, e As EventArgs) Handles EndShowComponentButton.Click
        Me.C.setIsValid(True)
        Me.Dispose()
    End Sub

    Private Sub CopiedComponentKCVTextBox_TextChanged(sender As Object, e As EventArgs) Handles CopiedComponentKCVTextBox.TextChanged
        If Not Regex.Match(CopiedComponentKCVTextBox.Text, "^[0-9,A-F]*$", RegexOptions.IgnoreCase).Success Then
            CopiedComponentKCVFormatLabel.Text = "Solo usar: 0-9, A-F"
        Else
            CopiedComponentKCVFormatLabel.Text = ""
            If Len(CopiedComponentKCVTextBox.Text) = 6 Then
                If OriginalComponentKCVTextBox.Text <> CopiedComponentKCVTextBox.Text Then
                    CopiedComponentKCVFormatLabel.Text = "KCV no coincide"
                    EndShowComponentButton.Enabled = False
                    Condition2 = False
                Else
                    Condition2 = True
                    ValidateConditions()
                End If
            Else
                EndShowComponentButton.Enabled = False
                Condition2 = False
            End If
        End If
    End Sub

    Private Sub CopiedKeyKCVTextBox_TextChanged(sender As Object, e As EventArgs) Handles CopiedKeyKCVTextBox.TextChanged
        If Not Regex.Match(CopiedKeyKCVTextBox.Text, "^[0-9,A-F]*$", RegexOptions.IgnoreCase).Success Then
            CopiedKeyKCVFormatLabel.Text = "Solo usar: 0-9, A-F"
        Else
            CopiedKeyKCVFormatLabel.Text = ""
            If Len(CopiedKeyKCVTextBox.Text) = 6 Then
                If OriginalKeyKCVTextBox.Text <> CopiedKeyKCVTextBox.Text Then
                    CopiedKeyKCVFormatLabel.Text = "KCV no coincide"
                    EndShowComponentButton.Enabled = False
                    Condition3 = False
                Else
                    Condition3 = True
                    ValidateConditions()
                End If
            Else
                EndShowComponentButton.Enabled = False
                Condition3 = False
            End If
        End If
    End Sub

    Private Sub CopiedKeyLeftKCVTextBox_TextChanged(sender As Object, e As EventArgs) Handles CopiedKeyLeftKCVTextBox.TextChanged
        If Not Regex.Match(CopiedKeyLeftKCVTextBox.Text, "^[0-9,A-F]*$", RegexOptions.IgnoreCase).Success Then
            CopiedKeyLeftKCVFormatLabel.Text = "Solo usar: 0-9, A-F"
        Else
            CopiedKeyLeftKCVFormatLabel.Text = ""
            If Len(CopiedKeyLeftKCVTextBox.Text) = 6 Then
                If OriginalKeyLeftKCVTextBox.Text <> CopiedKeyLeftKCVTextBox.Text Then
                    CopiedKeyLeftKCVFormatLabel.Text = "KCV no coincide"
                    EndShowComponentButton.Enabled = False
                    Condition4 = False
                Else
                    Condition4 = True
                    ValidateConditions()
                End If
            Else
                EndShowComponentButton.Enabled = False
                Condition4 = False
            End If
        End If
    End Sub

    Private Sub CopiedKeyRightKCVTextBox_TextChanged(sender As Object, e As EventArgs) Handles CopiedKeyRightKCVTextBox.TextChanged
        If Not Regex.Match(CopiedKeyRightKCVTextBox.Text, "^[0-9,A-F]*$", RegexOptions.IgnoreCase).Success Then
            CopiedKeyRightKCVFormatLabel.Text = "Solo usar: 0-9, A-F"
        Else
            CopiedKeyRightKCVFormatLabel.Text = ""
            If Len(CopiedKeyRightKCVTextBox.Text) = 6 Then
                If OriginalKeyRightKCVTextBox.Text <> CopiedKeyRightKCVTextBox.Text Then
                    CopiedKeyRightKCVFormatLabel.Text = "KCV no coincide"
                    EndShowComponentButton.Enabled = False
                    Condition5 = False
                Else
                    Condition5 = True
                    ValidateConditions()
                End If
            Else
                EndShowComponentButton.Enabled = False
                Condition5 = False
            End If
        End If
    End Sub

    Private Sub CopiedComponentTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles CopiedComponentTextBox.KeyDown,
        CopiedComponentKCVTextBox.KeyDown,
        CopiedKeyKCVTextBox.KeyDown,
        CopiedKeyLeftKCVTextBox.KeyDown,
        CopiedKeyRightKCVTextBox.KeyDown
        If e.Modifiers = Keys.Control AndAlso e.KeyCode = Keys.V Then
            e.SuppressKeyPress = True
        End If
    End Sub
End Class