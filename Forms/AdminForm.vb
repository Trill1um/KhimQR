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

    Private Sub simplifiedViewCheck_CheckedChanged(sender As Object, e As EventArgs) Handles simplifiedViewCheck.CheckedChanged
        LoadData()
    End Sub

    Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles resetButton.Click
        Dim confirm = MessageBox.Show("This will reset the database and delete all generated QR code images. Are you sure?", "Reset DB", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
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

            Dim simplified = simplifiedViewCheck.Checked
            LoadProf(simplified)
            LoadCourse(simplified)
            LoadStudent(simplified)
            LoadClassSection(simplified)
            LoadClassSession(simplified)
            LoadEnrollment(simplified)
            LoadAttendance(simplified)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Admin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Function LoadTable(sql As String) As DataTable
        Dim table As New DataTable()
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                For i As Integer = 0 To reader.FieldCount - 1
                    table.Columns.Add(reader.GetName(i), GetType(Object))
                Next

                While reader.Read()
                    Dim row = table.NewRow()
                    For i As Integer = 0 To reader.FieldCount - 1
                        row(i) = If(reader.IsDBNull(i), DBNull.Value, reader.GetValue(i))
                    Next
                    table.Rows.Add(row)
                End While
            End Using
        End Using
        Return table
    End Function

    Private Sub LoadProf(simplified As Boolean)
        Dim sql As String = If(simplified,
            "SELECT FirstName || ' ' || COALESCE(MiddleName || ' ', '') || LastName AS ProfessorName FROM Professor ORDER BY LastName, FirstName",
            "SELECT * FROM Professor")
        BindTable(profGrid, LoadTable(sql))
    End Sub

    Private Sub LoadCourse(simplified As Boolean)
        Dim sql As String = If(simplified,
            "SELECT Code, Name FROM Course ORDER BY Code",
            "SELECT * FROM Course")
        BindTable(courseGrid, LoadTable(sql))
    End Sub

    Private Sub LoadStudent(simplified As Boolean)
        Dim sql As String = If(simplified,
            "SELECT Student_Code, FirstName, MiddleName, LastName FROM Student ORDER BY LastName, FirstName",
            "SELECT * FROM Student")
        BindTable(studentGrid, LoadTable(sql))
    End Sub

    Private Sub LoadClassSection(simplified As Boolean)
        Dim sql As String = If(simplified,
            "SELECT c.Code AS CourseCode, c.Name AS CourseName, p.FirstName || ' ' || COALESCE(p.MiddleName || ' ', '') || p.LastName AS ProfessorName, cs.SectionName, cs.GracePeriodMinutes FROM ClassSection cs JOIN Course c ON cs.Course_ID = c.Course_ID JOIN Professor p ON cs.Professor_ID = p.Professor_ID ORDER BY c.Code, cs.SectionName",
            "SELECT * FROM ClassSection")
        BindTable(classSectionGrid, LoadTable(sql))
    End Sub

    Private Sub LoadClassSession(simplified As Boolean)
        Dim sql As String = If(simplified,
            "SELECT c.Code AS CourseCode, c.Name AS CourseName, p.FirstName || ' ' || COALESCE(p.MiddleName || ' ', '') || p.LastName AS ProfessorName, cs.SectionName, CASE sess.DayOfWeek WHEN 0 THEN 'Sunday' WHEN 1 THEN 'Monday' WHEN 2 THEN 'Tuesday' WHEN 3 THEN 'Wednesday' WHEN 4 THEN 'Thursday' WHEN 5 THEN 'Friday' ELSE 'Saturday' END AS DayOfWeek, sess.StartTime, sess.EndTime FROM ClassSession sess JOIN ClassSection cs ON sess.ClassSection_ID = cs.ClassSection_ID JOIN Course c ON cs.Course_ID = c.Course_ID JOIN Professor p ON cs.Professor_ID = p.Professor_ID ORDER BY c.Code, cs.SectionName, sess.DayOfWeek, sess.StartTime",
            "SELECT * FROM ClassSession")
        BindTable(classSessionGrid, LoadTable(sql))
    End Sub

    Private Sub LoadEnrollment(simplified As Boolean)
        Dim sql As String = If(simplified,
            "SELECT s.Student_Code, s.FirstName || ' ' || COALESCE(s.MiddleName || ' ', '') || s.LastName AS StudentName, c.Code AS CourseCode, c.Name AS CourseName, cs.SectionName, e.EnrollmentDate FROM Enrollment e JOIN Student s ON e.Student_ID = s.ID JOIN ClassSection cs ON e.ClassSection_ID = cs.ClassSection_ID JOIN Course c ON cs.Course_ID = c.Course_ID ORDER BY s.LastName, s.FirstName, c.Code, cs.SectionName",
            "SELECT * FROM Enrollment")
        BindTable(enrollmentGrid, LoadTable(sql))
    End Sub

    Private Sub LoadAttendance(simplified As Boolean)
        Dim sql As String = If(simplified,
            "SELECT s.Student_Code, s.FirstName || ' ' || COALESCE(s.MiddleName || ' ', '') || s.LastName AS StudentName, c.Code AS CourseCode, c.Name AS CourseName, cs.SectionName, sess.DayOfWeek, sess.StartTime, sess.EndTime, a.Date_Stamp, a.TimeIn, a.Status FROM Attendance a JOIN Enrollment e ON a.Enrollment_ID = e.Enrollment_ID JOIN Student s ON e.Student_ID = s.ID JOIN ClassSession sess ON a.ClassSession_ID = sess.ClassSession_ID JOIN ClassSection cs ON sess.ClassSection_ID = cs.ClassSection_ID JOIN Course c ON cs.Course_ID = c.Course_ID ORDER BY a.Date_Stamp DESC, c.Code, cs.SectionName, sess.StartTime",
            "SELECT * FROM Attendance")
        BindTable(attendanceGrid, LoadTable(sql))
    End Sub

    Private Sub BindTable(grid As DataGridView, table As DataTable)
        grid.DataSource = Nothing
        grid.DataSource = table
    End Sub
End Class
