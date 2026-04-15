Public Class MainForm
    Private Enum UserRole
        None
        Admin
        Professor
    End Enum

    Private pageManager As PageNavigationManager
    Private currentRole As UserRole = UserRole.None
    Private currentProfessorId As Integer = 0

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pageManager = New PageNavigationManager(pageHostPanel)
        logoutButton.Visible = False
        ShowLoginPage()
    End Sub

    Private Sub homeButton_Click(sender As Object, e As EventArgs) Handles homeButton.Click
        ShowHomePage()
    End Sub

    Private Sub logoutButton_Click(sender As Object, e As EventArgs) Handles logoutButton.Click
        currentRole = UserRole.None
        currentProfessorId = 0
        ShowLoginPage()
    End Sub

    Private Sub ShowLoginPage()
        Dim loginPage As New RoleLoginForm()
        AddHandler loginPage.LoginSucceeded, AddressOf RoleLogin_LoginSucceeded

        pageManager.Navigate(loginPage)
        currentPageLabel.Text = "Login"
        logoutButton.Visible = False
    End Sub

    Private Sub RoleLogin_LoginSucceeded(sender As Object, e As RoleLoginEventArgs)
        If e.IsAdmin Then
            currentRole = UserRole.Admin
            currentProfessorId = 0
        Else
            currentRole = UserRole.Professor
            currentProfessorId = e.SelectedProfessorId.GetValueOrDefault()
        End If

        logoutButton.Visible = True
        ShowHomePage()
    End Sub

    Private Sub ShowHomePage()
        Select Case currentRole
            Case UserRole.Admin
                Dim adminHome As New AdminHomeForm()
                AddHandler adminHome.ShowAdminRequested, AddressOf Home_ShowAdminRequested
                AddHandler adminHome.ShowAddRequested, AddressOf Home_ShowAddRequested
                AddHandler adminHome.ShowAttendanceRequested, AddressOf Home_ShowAttendanceRequested
                AddHandler adminHome.ShowEnrollmentRequested, AddressOf Home_ShowEnrollmentRequested
                AddHandler adminHome.ShowProfessorPanelRequested, AddressOf Home_ShowProfessorPanelRequested
                pageManager.Navigate(adminHome)
                currentPageLabel.Text = "Admin Home"

            Case UserRole.Professor
                Dim professorHome As New ProfessorHomeForm()
                AddHandler professorHome.ShowAttendanceRequested, AddressOf Home_ShowAttendanceRequested
                AddHandler professorHome.ShowProfessorPanelRequested, AddressOf Home_ShowProfessorPanelRequested
                pageManager.Navigate(professorHome)
                currentPageLabel.Text = "Professor Home"

            Case Else
                ShowLoginPage()
        End Select
    End Sub

    Private Sub Home_ShowAdminRequested(sender As Object, e As EventArgs)
        pageManager.Navigate(New AdminForm())
        currentPageLabel.Text = "Admin"
    End Sub

    Private Sub Home_ShowAddRequested(sender As Object, e As EventArgs)
        pageManager.Navigate(New AddForm())
        currentPageLabel.Text = "Add Data"
    End Sub

    Private Sub Home_ShowEnrollmentRequested(sender As Object, e As EventArgs)
        pageManager.Navigate(New EnrollmentForm())
        currentPageLabel.Text = "Enrollment"
    End Sub

    Private Sub Home_ShowProfessorPanelRequested(sender As Object, e As EventArgs)
        Dim panel As New ProfessorPanelForm() With {
            .CurrentProfessorId = currentProfessorId
        }
        pageManager.Navigate(panel)
        currentPageLabel.Text = "Professor Panel"
    End Sub

    Private Sub Home_ShowAttendanceRequested(sender As Object, e As EventArgs)
        If currentProfessorId <= 0 Then
            MessageBox.Show("Attendance requires a professor login. Please login as a professor.", "Attendance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim scanner As New AttendanceForm() With {
            .CurrentProfessorId = currentProfessorId
        }
        pageManager.Navigate(scanner)
        currentPageLabel.Text = "Attendance"
    End Sub
End Class
