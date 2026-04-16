Public Class AdminHomeForm
    Public Event ShowAdminRequested As EventHandler
    Public Event ShowAddRequested As EventHandler
    Public Event ShowAttendanceRequested As EventHandler
    Public Event ShowEnrollmentRequested As EventHandler
    Public Event ShowProfessorPanelRequested As EventHandler

    Private Sub adminButton_Click(sender As Object, e As EventArgs) Handles adminButton.Click
        RaiseEvent ShowAdminRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub addButton_Click(sender As Object, e As EventArgs) Handles addButton.Click
        RaiseEvent ShowAddRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub enrollmentButton_Click(sender As Object, e As EventArgs) Handles enrollmentButton.Click
        RaiseEvent ShowEnrollmentRequested(Me, EventArgs.Empty)
    End Sub
End Class