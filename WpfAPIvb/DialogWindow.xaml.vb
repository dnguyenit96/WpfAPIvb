Public Class DialogWindow
    Public _DialogMessage As String

    Private Sub btnAccept_Click(sender As Object, e As RoutedEventArgs)
        Close()
    End Sub

    Private Sub Window_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Enter Or e.Key = Key.Escape Then
            Close()
        End If
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Me.tbxMain.Text = _DialogMessage
    End Sub
End Class
