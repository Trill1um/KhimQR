Imports System.IO
Imports Microsoft.Data.Sqlite

Public Class AdminForm
    Inherits Form

    Private ReadOnly tabControl As New TabControl()
    Private ReadOnly profPage As New TabPage("Professor")
    Private ReadOnly coursePage As New TabPage("Course")
    Private ReadOnly studentPage As New TabPage("Student")
    Private ReadOnly classSectionPage As New TabPage("ClassSection")
    Private ReadOnly classSessionPage As New TabPage("ClassSession")
    Private ReadOnly enrollmentPage As New TabPage("Enrollment")
    Private ReadOnly attendancePage As New TabPage("Attendance")

    Private ReadOnly profGrid As New DataGridView()
    Private ReadOnly courseGrid As New DataGridView()
    Private ReadOnly studentGrid As New DataGridView()
    Private ReadOnly classSectionGrid As New DataGridView()
    Private ReadOnly classSessionGrid As New DataGridView()
    Private ReadOnly enrollmentGrid As New DataGridView()
    Private ReadOnly attendanceGrid As New DataGridView()
    Private ReadOnly refreshButton As New Button()
    Private ReadOnly resetButton As New Button()

    Public Sub New()
        Text = "Admin - Database Viewer"
        Width = 950
        Height = 550
        StartPosition = FormStartPosition.CenterParent

        refreshButton.Text = "Refresh"
        refreshButton.Width = 120
        refreshButton.Height = 32
        refreshButton.Top = 10
        refreshButton.Left = 10

        resetButton.Text = "Reset DB"
        resetButton.Width = 120
        resetButton.Height = 32
        resetButton.Top = 10
        resetButton.Left = refreshButton.Right + 10

        tabControl.Left = 10
        tabControl.Top = refreshButton.Bottom + 10
        tabControl.Width = ClientSize.Width - 20
        tabControl.Height = ClientSize.Height - tabControl.Top - 10
        tabControl.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right

        ConfigureGrid(profGrid)
        ConfigureGrid(courseGrid)
        ConfigureGrid(studentGrid)
        ConfigureGrid(classSectionGrid)
        ConfigureGrid(classSessionGrid)
        ConfigureGrid(enrollmentGrid)
        ConfigureGrid(attendanceGrid)

        profPage.Controls.Add(profGrid)
        coursePage.Controls.Add(courseGrid)
        studentPage.Controls.Add(studentGrid)
        classSectionPage.Controls.Add(classSectionGrid)
        classSessionPage.Controls.Add(classSessionGrid)
        enrollmentPage.Controls.Add(enrollmentGrid)
        attendancePage.Controls.Add(attendanceGrid)

        tabControl.TabPages.Add(profPage)
        tabControl.TabPages.Add(coursePage)
        tabControl.TabPages.Add(studentPage)
        tabControl.TabPages.Add(classSectionPage)
        tabControl.TabPages.Add(classSessionPage)
        tabControl.TabPages.Add(enrollmentPage)
        tabControl.TabPages.Add(attendancePage)

        Controls.Add(refreshButton)
        Controls.Add(tabControl)
        Controls.Add(resetButton)

        AddHandler resetButton.Click, AddressOf ResetButton_Click
        AddHandler refreshButton.Click, AddressOf RefreshButton_Click
        AddHandler Load, AddressOf AdminForm_Load
    End Sub

    Private Sub ConfigureGrid(grid As DataGridView)
        grid.Dock = DockStyle.Fill
        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.MultiSelect = False
    End Sub

    Private Sub AdminForm_Load(sender As Object, e As EventArgs)
        LoadData()
    End Sub

    Private Sub ResetButton_Click(sender As Object, e As EventArgs)
        Dim confirm = MessageBox.Show("This will reset the database and delete all generated QR code images. Are you sure?", "Reset DB",
                                  MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If confirm <> DialogResult.Yes Then Return

        Try
            Dim sql = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Sql", "999_Reset_KhimQR_Database.sql"))
            Using cmd As New SqliteCommand(sql, sqlconn)
                cmd.ExecuteNonQuery()
            End Using

            ' Delete all exported QR code images
            Dim qrStorageDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "QrCodes")
            If Directory.Exists(qrStorageDir) Then
                For Each file In Directory.GetFiles(qrStorageDir, "*.png")
                    Try
                        IO.File.Delete(file)
                    Catch ignore As Exception
                    End Try
                Next
            End If

            LoadData()
            MessageBox.Show("Database and images reset successfully.", "Reset DB", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Reset DB", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs)
        LoadData()
    End Sub

    Private Sub LoadData()
        Try
            If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
                connect()
            End If

            LoadProf()
            LoadCourse()
            LoadStudent()
            LoadClassSection()
            LoadClassSession()
            LoadEnrollment()
            LoadAttendance()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Admin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub LoadProf()
        Dim table As New DataTable()
        Const sql As String = "SELECT * FROM Professor"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using
        profGrid.DataSource = table
    End Sub

    Private Sub LoadCourse()
        Dim table As New DataTable()
        Const sql As String = "SELECT * FROM Course"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using
        courseGrid.DataSource = table
    End Sub

    Private Sub LoadStudent()
        Dim table As New DataTable()
        Const sql As String = "SELECT * FROM Student"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using
        studentGrid.DataSource = table
    End Sub

    Private Sub LoadClassSection()
        Dim table As New DataTable()
        Const sql As String = "SELECT * FROM ClassSection"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using
        classSectionGrid.DataSource = table
    End Sub

    Private Sub LoadClassSession()
        Dim table As New DataTable()
        Const sql As String = "SELECT * FROM ClassSession"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using
        classSessionGrid.DataSource = table
    End Sub

    Private Sub LoadEnrollment()
        Dim table As New DataTable()
        Const sql As String = "SELECT * FROM Enrollment"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using
        enrollmentGrid.DataSource = table
    End Sub

    Private Sub LoadAttendance()
        Dim table As New DataTable()
        Const sql As String = "SELECT * FROM Attendance"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using
        attendanceGrid.DataSource = table
    End Sub
    Private Sub InitializeComponent()
        SuspendLayout()
        ' 
        ' AdminForm
        ' 
        ClientSize = New Size(282, 253)
        Name = "AdminForm"
        StartPosition = FormStartPosition.CenterParent
        ResumeLayout(False)

    End Sub

End Class
