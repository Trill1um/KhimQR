Imports System.IO
Imports Microsoft.Data.Sqlite

Public Class AdminForm
    Public Sub New()
        InitializeComponent()
        ConfigureGrid(profGrid)
        ConfigureGrid(courseGrid)
        ConfigureGrid(studentGrid)
        ConfigureGrid(classSectionGrid)
        ConfigureGrid(classSessionGrid)
        ConfigureGrid(enrollmentGrid)
        ConfigureGrid(attendanceGrid)
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

    Private Sub AdminForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
    End Sub

    Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles resetButton.Click
        Dim confirm = MessageBox.Show("This will reset the database and delete all generated QR code images. Are you sure?", "Reset DB",
                                  MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If confirm <> DialogResult.Yes Then Return

        Try
            Dim qrStorageDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "QrCodes")
            If Directory.Exists(qrStorageDir) Then
                For Each file In Directory.GetFiles(qrStorageDir, "*.png")
                    Try
                        IO.File.Delete(file)
                    Catch ignore As Exception
                    End Try
                Next
            End If

            ResetAndInitializeDatabase()

            LoadData()
            MessageBox.Show("Database and images reset successfully.", "Reset DB", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Reset DB", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles refreshButton.Click
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
End Class
