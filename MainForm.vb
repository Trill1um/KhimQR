Public Class MainForm
    Private pageManager As PageNavigationManager

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pageManager = New PageNavigationManager(pageHostPanel)
        ShowHomePage()
    End Sub

    Private Sub homeButton_Click(sender As Object, e As EventArgs) Handles homeButton.Click
        ShowHomePage()
    End Sub

    Private Sub ShowHomePage()
        Dim home As New HomeForm()
        AddHandler home.ShowAdminRequested, AddressOf Home_ShowAdminRequested
        AddHandler home.ShowAddRequested, AddressOf Home_ShowAddRequested
        AddHandler home.ShowAttendanceRequested, AddressOf Home_ShowAttendanceRequested
        pageManager.Navigate(home)
        currentPageLabel.Text = "Home"
    End Sub

    Private Sub Home_ShowAdminRequested(sender As Object, e As EventArgs)
        pageManager.Navigate(New AdminForm())
        currentPageLabel.Text = "Admin"
    End Sub

    Private Sub Home_ShowAddRequested(sender As Object, e As EventArgs)
        pageManager.Navigate(New AddForm())
        currentPageLabel.Text = "Add Data"
    End Sub

    Private Sub Home_ShowAttendanceRequested(sender As Object, e As EventArgs)
        Using loginForm As New ProfessorLoginForm()
            If loginForm.ShowDialog(Me) = DialogResult.OK Then
                Dim scanner As New Form2() With {
                    .CurrentProfessorId = loginForm.SelectedProfessorId
                }
                pageManager.Navigate(scanner)
                currentPageLabel.Text = "Attendance"
            End If
        End Using
    End Sub
End Class
