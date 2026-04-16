Public Class ProfessorHomeForm
    Public Event ShowAttendanceRequested As EventHandler
    Public Event ShowProfessorPanelRequested As EventHandler
    Public Event ShowProfessorAttendancePanelRequested As EventHandler

    Private Sub attendanceButton_Click(sender As Object, e As EventArgs) Handles attendanceButton.Click
        RaiseEvent ShowAttendanceRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub professorPanelButton_Click(sender As Object, e As EventArgs) Handles professorPanelButton.Click
        RaiseEvent ShowProfessorPanelRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub attendancePanelButton_Click(sender As Object, e As EventArgs) Handles attendancePanelButton.Click
        RaiseEvent ShowProfessorAttendancePanelRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub ProfessorHomeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class