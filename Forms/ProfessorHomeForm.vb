Public Class ProfessorHomeForm
    Public Event ShowAttendanceRequested As EventHandler
    Public Event ShowProfessorPanelRequested As EventHandler

    Private Sub attendanceButton_Click(sender As Object, e As EventArgs) Handles attendanceButton.Click
        RaiseEvent ShowAttendanceRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub professorPanelButton_Click(sender As Object, e As EventArgs) Handles professorPanelButton.Click
        RaiseEvent ShowProfessorPanelRequested(Me, EventArgs.Empty)
    End Sub
End Class