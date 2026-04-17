Imports Microsoft.Data.Sqlite

Public Class ProfessorAttendancePanelForm
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property CurrentProfessorId As Integer

    Private isLoading As Boolean

    Public Sub New()
        InitializeComponent()
        ConfigureGrid()
        AddHandler attendanceGrid.DataError, AddressOf attendanceGrid_DataError
    End Sub

    Private Sub ConfigureGrid()
        attendanceGrid.ReadOnly = True
        attendanceGrid.AllowUserToAddRows = False
        attendanceGrid.AllowUserToDeleteRows = False
        attendanceGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        attendanceGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        attendanceGrid.MultiSelect = False
        attendanceGrid.AutoGenerateColumns = False
        attendanceGrid.Columns.Clear()

        AddTextColumn("StudentCode", "Student Code")
        AddTextColumn("StudentName", "Student Name")
        AddTextColumn("CourseCode", "Course")
        AddTextColumn("CourseName", "Course Name")
        AddTextColumn("SectionName", "Section")
        AddTextColumn("SessionDay", "Session Day")
        AddTextColumn("SessionStart", "Start")
        AddTextColumn("SessionEnd", "End")
        AddTextColumn("AttendanceDate", "Date")
        AddTextColumn("TimeIn", "Time In")
        AddTextColumn("Status", "Status")
    End Sub

    Private Sub AddTextColumn(dataPropertyName As String, headerText As String)
        Dim col As New DataGridViewTextBoxColumn() With {
            .DataPropertyName = dataPropertyName,
            .Name = dataPropertyName,
            .HeaderText = headerText,
            .SortMode = DataGridViewColumnSortMode.Automatic,
            .ReadOnly = True
        }
        attendanceGrid.Columns.Add(col)
    End Sub

    Private Sub ProfessorAttendancePanelForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        isLoading = True
        Try
            attendanceDatePicker.Value = SystemClock.Today
            LoadCourses()
            LoadSessions()
            LoadAttendanceRecords()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Attendance Panel", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Finally
            isLoading = False
        End Try

        QueueAutoAbsentFinalization()
    End Sub

    Private Sub LoadCourses()
        EnsureConnection()

        Dim rawTable As New DataTable()
        Const sql As String = "SELECT DISTINCT c.Course_ID, c.Code || ' - ' || c.Name AS DisplayText " &
                            "FROM ClassSection cs " &
                            "JOIN Course c ON cs.Course_ID = c.Course_ID " &
                            "WHERE (@profId = 0 OR cs.Professor_ID = @profId) " &
                            "ORDER BY c.Code"

        Using cmd As New SqliteCommand(sql, sqlconn)
            cmd.Parameters.AddWithValue("@profId", CurrentProfessorId)
            Using reader = cmd.ExecuteReader()
                rawTable.Load(reader)
            End Using
        End Using

        Dim table As New DataTable()
        table.Columns.Add("Course_ID", GetType(Integer))
        table.Columns.Add("DisplayText", GetType(String))
        table.Rows.Add(DBNull.Value, String.Empty)

        For Each row As DataRow In rawTable.Rows
            table.Rows.Add(Convert.ToInt32(row("Course_ID")), Convert.ToString(row("DisplayText")))
        Next

        courseComboBox.DataSource = table
        courseComboBox.DisplayMember = "DisplayText"
        courseComboBox.ValueMember = "Course_ID"
        courseComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadSessions(Optional courseId As Integer? = Nothing)
        EnsureConnection()

        Dim rawTable As New DataTable()
        Dim sql As String = "SELECT sess.ClassSession_ID, " &
                            "c.Code || ' - ' || cs.SectionName || ' | ' || " &
                            "CASE sess.DayOfWeek " &
                            "WHEN 0 THEN 'Sunday' WHEN 1 THEN 'Monday' WHEN 2 THEN 'Tuesday' WHEN 3 THEN 'Wednesday' " &
                            "WHEN 4 THEN 'Thursday' WHEN 5 THEN 'Friday' ELSE 'Saturday' END || " &
                            "' ' || sess.StartTime || '-' || sess.EndTime AS DisplayText " &
                            "FROM ClassSession sess " &
                            "JOIN ClassSection cs ON sess.ClassSection_ID = cs.ClassSection_ID " &
                            "JOIN Course c ON cs.Course_ID = c.Course_ID " &
                            "WHERE (@profId = 0 OR cs.Professor_ID = @profId)"

        If courseId.HasValue Then
            sql &= " AND cs.Course_ID = @courseId"
        End If

        sql &= " ORDER BY c.Code, cs.SectionName, sess.DayOfWeek, sess.StartTime"

        Using cmd As New SqliteCommand(sql, sqlconn)
            cmd.Parameters.AddWithValue("@profId", CurrentProfessorId)
            If courseId.HasValue Then
                cmd.Parameters.AddWithValue("@courseId", courseId.Value)
            End If

            Using reader = cmd.ExecuteReader()
                rawTable.Load(reader)
            End Using
        End Using

        Dim table As New DataTable()
        table.Columns.Add("ClassSession_ID", GetType(Integer))
        table.Columns.Add("DisplayText", GetType(String))
        table.Rows.Add(DBNull.Value, String.Empty)

        For Each row As DataRow In rawTable.Rows
            table.Rows.Add(Convert.ToInt32(row("ClassSession_ID")), Convert.ToString(row("DisplayText")))
        Next

        sessionComboBox.DataSource = table
        sessionComboBox.DisplayMember = "DisplayText"
        sessionComboBox.ValueMember = "ClassSession_ID"
        sessionComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadAttendanceRecords(Optional courseId As Integer? = Nothing, Optional classSessionId As Integer? = Nothing)
        EnsureConnection()

        Dim selectedDate As DateTime = attendanceDatePicker.Value.Date
        Dim selectedDay As Integer = CInt(selectedDate.DayOfWeek)
        Dim dateStamp As String = selectedDate.ToString("yyyy-MM-dd")
        Dim displayDate As String = selectedDate.ToString("MMMM dd, yyyy")
        Dim todayStamp As String = SystemClock.Now.Date.ToString("yyyy-MM-dd")
        Dim currentTime As String = SystemClock.Now.ToString("HH:mm")

        Dim table As New DataTable()
        Const sql As String = "WITH session_scope AS (" &
                              "    SELECT sess.ClassSession_ID, cs.ClassSection_ID, c.Code AS CourseCode, c.Name AS CourseName, cs.SectionName, " &
                              "           CASE sess.DayOfWeek WHEN 0 THEN 'Sunday' WHEN 1 THEN 'Monday' WHEN 2 THEN 'Tuesday' WHEN 3 THEN 'Wednesday' WHEN 4 THEN 'Thursday' WHEN 5 THEN 'Friday' ELSE 'Saturday' END AS SessionDay, " &
                              "           sess.DayOfWeek, sess.StartTime, sess.EndTime, " &
                              "           CASE WHEN @selectedDate < @today THEN 1 WHEN @selectedDate > @today THEN 0 WHEN sess.EndTime <= @currentTime THEN 1 ELSE 0 END AS IsFinished " &
                              "    FROM ClassSession sess " &
                              "    JOIN ClassSection cs ON sess.ClassSection_ID = cs.ClassSection_ID " &
                              "    JOIN Course c ON cs.Course_ID = c.Course_ID " &
                              "    WHERE (@profId = 0 OR cs.Professor_ID = @profId) " &
                              "    AND sess.DayOfWeek = @dayOfWeek " &
                              "    AND (@courseId IS NULL OR cs.Course_ID = @courseId) " &
                              "    AND (@classSessionId IS NULL OR sess.ClassSession_ID = @classSessionId) " &
                              ") " &
                              "SELECT ss.CourseCode, ss.CourseName, ss.SectionName, ss.SessionDay, ss.StartTime AS SessionStart, ss.EndTime AS SessionEnd, @displayDate AS AttendanceDate, " &
                              "       s.Student_Code AS StudentCode, " &
                              "       (s.FirstName || ' ' || CASE WHEN IFNULL(TRIM(s.MiddleName), '') = '' THEN '' ELSE SUBSTR(TRIM(s.MiddleName), 1, 1) || '. ' END || s.LastName) AS StudentName, " &
                              "       IFNULL(a.TimeIn, '') AS TimeIn, COALESCE(a.Status, 'Absent') AS Status " &
                              "FROM session_scope ss " &
                              "JOIN Enrollment e ON ss.ClassSection_ID = e.ClassSection_ID " &
                              "JOIN Student s ON e.Student_ID = s.ID " &
                              "LEFT JOIN Attendance a ON a.ClassSession_ID = ss.ClassSession_ID AND a.Enrollment_ID = e.Enrollment_ID AND a.Date_Stamp = @dateStamp " &
                              "WHERE ss.IsFinished = 1 AND e.EnrollmentDate <= @dateStamp " &
                              "UNION ALL " &
                              "SELECT ss.CourseCode, ss.CourseName, ss.SectionName, ss.SessionDay, ss.StartTime AS SessionStart, ss.EndTime AS SessionEnd, @displayDate AS AttendanceDate, " &
                              "       s.Student_Code AS StudentCode, " &
                              "       (s.FirstName || ' ' || CASE WHEN IFNULL(TRIM(s.MiddleName), '') = '' THEN '' ELSE SUBSTR(TRIM(s.MiddleName), 1, 1) || '. ' END || s.LastName) AS StudentName, " &
                              "       IFNULL(a.TimeIn, '') AS TimeIn, a.Status AS Status " &
                              "FROM session_scope ss " &
                              "JOIN Attendance a ON a.ClassSession_ID = ss.ClassSession_ID AND a.Date_Stamp = @dateStamp " &
                              "JOIN Enrollment e ON a.Enrollment_ID = e.Enrollment_ID AND e.EnrollmentDate <= @dateStamp " &
                              "JOIN Student s ON e.Student_ID = s.ID " &
                              "WHERE ss.IsFinished = 0 " &
                              "ORDER BY CourseCode, SectionName, SessionStart, StudentName"

        Using cmd As New SqliteCommand(sql, sqlconn)
            cmd.Parameters.AddWithValue("@profId", CurrentProfessorId)
            cmd.Parameters.AddWithValue("@dateStamp", dateStamp)
            cmd.Parameters.AddWithValue("@displayDate", displayDate)
            cmd.Parameters.AddWithValue("@dayOfWeek", selectedDay)
            cmd.Parameters.AddWithValue("@selectedDate", dateStamp)
            cmd.Parameters.AddWithValue("@today", todayStamp)
            cmd.Parameters.AddWithValue("@currentTime", currentTime)

            Dim courseParam As New SqliteParameter("@courseId", DbType.Int32)
            courseParam.Value = If(courseId.HasValue, courseId.Value, CType(DBNull.Value, Object))
            cmd.Parameters.Add(courseParam)

            Dim sessionParam As New SqliteParameter("@classSessionId", DbType.Int32)
            sessionParam.Value = If(classSessionId.HasValue, classSessionId.Value, CType(DBNull.Value, Object))
            cmd.Parameters.Add(sessionParam)

            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using

        attendanceGrid.DataSource = Nothing
        attendanceGrid.DataSource = table
    End Sub

    Private Sub QueueAutoAbsentFinalization()
        Dim selectedDate As DateTime = attendanceDatePicker.Value.Date
        Dim courseId = GetSelectedInt(courseComboBox)
        Dim sessionId = GetSelectedInt(sessionComboBox)

        Task.Run(Sub()
                     Try
                         AttendanceAutoAbsentService.FinalizeProfessorAbsences(CurrentProfessorId, selectedDate, courseId, sessionId)
                     Catch
                     End Try

                     If IsHandleCreated AndAlso Not IsDisposed Then
                         BeginInvoke(Sub()
                                         If Not IsDisposed Then
                                             LoadAttendanceRecords(GetSelectedInt(courseComboBox), GetSelectedInt(sessionComboBox))
                                         End If
                                     End Sub)
                     End If
                 End Sub)
    End Sub

    Private Sub attendanceGrid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs)
        e.ThrowException = False
    End Sub

    Private Function GetSelectedInt(combo As ComboBox) As Integer?
        If combo.SelectedValue Is Nothing OrElse combo.SelectedValue Is DBNull.Value Then
            Return Nothing
        End If

        Dim value As Integer
        If Integer.TryParse(combo.SelectedValue.ToString(), value) Then
            Return value
        End If

        Return Nothing
    End Function

    Private Sub ApplyFilters()
        LoadAttendanceRecords(GetSelectedInt(courseComboBox), GetSelectedInt(sessionComboBox))
        If Not isLoading Then
            QueueAutoAbsentFinalization()
        End If
    End Sub

    Private Sub courseComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles courseComboBox.SelectedIndexChanged
        If isLoading Then
            Return
        End If

        LoadSessions(GetSelectedInt(courseComboBox))
        ApplyFilters()
    End Sub

    Private Sub sessionComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sessionComboBox.SelectedIndexChanged
        If isLoading Then
            Return
        End If

        ApplyFilters()
    End Sub

    Private Sub attendanceDatePicker_ValueChanged(sender As Object, e As EventArgs) Handles attendanceDatePicker.ValueChanged
        If isLoading Then
            Return
        End If

        ApplyFilters()
    End Sub

    Private Sub clearFiltersButton_Click(sender As Object, e As EventArgs) Handles clearFiltersButton.Click
        isLoading = True
        Try
            attendanceDatePicker.Value = SystemClock.Today
            courseComboBox.SelectedIndex = 0
            LoadSessions()
            sessionComboBox.SelectedIndex = 0
            LoadAttendanceRecords()
        Finally
            isLoading = False
        End Try

        QueueAutoAbsentFinalization()
    End Sub

    Private Sub EnsureConnection()
        If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
            connect()
        End If
    End Sub
End Class
