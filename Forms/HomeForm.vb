Public Class HomeForm
    Public Event ShowAdminRequested As EventHandler
    Public Event ShowAddRequested As EventHandler
    Public Event ShowAttendanceRequested As EventHandler
    Public Event ShowEnrollmentRequested As EventHandler

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RaiseEvent ShowAdminRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        RaiseEvent ShowAddRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        RaiseEvent ShowAttendanceRequested(Me, EventArgs.Empty)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        RaiseEvent ShowEnrollmentRequested(Me, EventArgs.Empty)
    End Sub
End Class