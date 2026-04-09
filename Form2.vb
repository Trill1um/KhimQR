Imports Microsoft.Data.Sqlite
Imports OpenCvSharp
Imports OpenCvSharp.Extensions

Public Class Form2
    Private camera As VideoCapture
    Private frameTimer As Timer
    Private latestFrame As Mat
    Private ReadOnly qrDetector As New QRCodeDetector()
    Private lastDecodeAttemptUtc As DateTime = DateTime.MinValue
    Private lastHandledStudentId As String = String.Empty
    Private lastHandledAtUtc As DateTime = DateTime.MinValue
    Private isShowingMessage As Boolean

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        frameTimer = New Timer() With {.Interval = 33}
        AddHandler frameTimer.Tick, AddressOf FrameTimer_Tick
        startCamera()
    End Sub

    Private Sub FrameTimer_Tick(sender As Object, e As EventArgs)
        If isShowingMessage Then
            Return
        End If

        If camera Is Nothing OrElse Not camera.IsOpened() Then
            Return
        End If

        Dim frame As New Mat()
        If camera.Read(frame) AndAlso Not frame.Empty() Then
            latestFrame?.Dispose()
            latestFrame = frame.Clone()

            Dim bmp = BitmapConverter.ToBitmap(frame)
            Dim old = PictureBox1.Image
            PictureBox1.Image = bmp
            old?.Dispose()

            If (DateTime.UtcNow - lastDecodeAttemptUtc).TotalMilliseconds >= 200 Then
                lastDecodeAttemptUtc = DateTime.UtcNow
                TryAutoScan(latestFrame)
            End If
        End If

        frame.Dispose()
    End Sub

    Private Sub TryAutoScan(frame As Mat)
        Try
            Dim points As Point2f() = Nothing
            Dim decoded = qrDetector.DetectAndDecode(frame, points)

            If String.IsNullOrWhiteSpace(decoded) Then
                Return
            End If

            decoded = decoded.Trim()

            If decoded.Equals(lastHandledStudentId, StringComparison.OrdinalIgnoreCase) AndAlso
               (DateTime.UtcNow - lastHandledAtUtc).TotalSeconds < 3 Then
                Return
            End If

            ' 1. Use decoded as Student_Code, get Student ID
            Dim studentRepo As New StudentRepository()
            Dim student = studentRepo.GetByStudentCode(sqlconn, decoded)

            If student Is Nothing Then
                TextBox1.Text = Nothing
                TextBox2.Text = Nothing
                TextBox4.Text = Nothing
                TextBox3.Text = Nothing
                TextBox6.Text = Nothing
                TextBox5.Text = Nothing
                Label7.Text = "Student Does Not" & Environment.NewLine & "Exist!"
                Return
            End If

            ' 2. Find currently Active ClassSession based on DateTime.Now (or prompt professor)
            ' For this generalized refactor, we find ANY Active ClassSession right now
            Dim now As DateTime = DateTime.Now
            Dim currentDayOfWeek As Integer = CInt(now.DayOfWeek)
            Dim currentTime As String = now.ToString("HH:mm")

            Dim activeSessionId As Integer = 0
            Dim activeSectionId As Integer = 0
            Dim gracePeriod As Integer = 0
            Dim startTime As DateTime
            Dim endTime As DateTime
            Dim courseName As String = ""
            Dim sectionName As String = ""

            Dim sql As String = "SELECT cs.ClassSession_ID, c.ClassSection_ID, c.GracePeriodMinutes, cs.StartTime, cs.EndTime, cr.Name, c.SectionName " &
                                "FROM ClassSession cs " &
                                "JOIN ClassSection c ON cs.ClassSection_ID = c.ClassSection_ID " &
                                "JOIN Course cr ON c.Course_ID = cr.Course_ID " &
                                "WHERE cs.DayOfWeek = @dow AND cs.StartTime <= @time AND cs.EndTime >= @time LIMIT 1"

            Using cmd As New SqliteCommand(sql, sqlconn)
                cmd.Parameters.AddWithValue("@dow", currentDayOfWeek)
                cmd.Parameters.AddWithValue("@time", currentTime)
                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then
                        activeSessionId = Convert.ToInt32(reader("ClassSession_ID"))
                        activeSectionId = Convert.ToInt32(reader("ClassSection_ID"))
                        gracePeriod = Convert.ToInt32(reader("GracePeriodMinutes"))
                        startTime = DateTime.Parse(reader("StartTime").ToString())
                        endTime = DateTime.Parse(reader("EndTime").ToString())
                        courseName = reader("Name").ToString()
                        sectionName = reader("SectionName").ToString()
                    End If
                End Using
            End Using

            If activeSessionId = 0 Then
                Label7.Text = "Class Not In Session"
                TextBox1.Text = student.Student_Code
                TextBox2.Text = BuildDisplayName(student.FirstName, student.MiddleName, student.LastName)
                lastHandledStudentId = decoded
                lastHandledAtUtc = DateTime.UtcNow
                Return
            End If

            ' 3. Query Enrollment table using Student_ID and activeSectionId
            Dim enrollmentRepo As New EnrollmentRepository()
            Dim enrollment = enrollmentRepo.GetEnrollment(sqlconn, student.ID, activeSectionId)

            If enrollment Is Nothing Then
                MsgBox("Student Not Enrolled in Current Class Session", MsgBoxStyle.Exclamation, "System Message")
                Label7.Text = "Not Enrolled"
                lastHandledStudentId = decoded
                lastHandledAtUtc = DateTime.UtcNow
                Return
            End If

            ' 4. Is Student already in Database? (prevent duplicates)
            Dim attendanceRepo As New AttendanceRepository()
            Dim dateStamp As String = now.ToString("yyyy-MM-dd")

            If attendanceRepo.HasRecord(sqlconn, enrollment.Enrollment_ID, activeSessionId, dateStamp) Then
                Label7.Text = "Student Already" & Environment.NewLine & "Recorded"
                lastHandledStudentId = decoded
                lastHandledAtUtc = DateTime.UtcNow
                Return
            End If

            ' 5. Determine Arrival status: Use exact DateTime and Grace Period
            Dim arrivalTime As DateTime = New DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second)
            Dim sessionStart As DateTime = New DateTime(now.Year, now.Month, now.Day, startTime.Hour, startTime.Minute, 0)

            Dim status As String = "Present"
            If arrivalTime > sessionStart.AddMinutes(gracePeriod) Then
                status = "Late"
            End If

            ' 6. Insert Attendance
            Dim timeInAsStr As String = now.ToString("HH:mm:ss")
            Dim att As New Attendance With {
                .ClassSession_ID = activeSessionId,
                .Enrollment_ID = enrollment.Enrollment_ID,
                .Date_Stamp = dateStamp,
                .TimeIn = timeInAsStr,
                .Status = status
            }

            attendanceRepo.RecordAttendance(sqlconn, att)

            Label7.Text = status & "!" & Environment.NewLine & "Attendance Recorded"

            ' Populate UX details
            TextBox1.Text = student.Student_Code
            TextBox2.Text = BuildDisplayName(student.FirstName, student.MiddleName, student.LastName)
            TextBox4.Text = courseName
            TextBox3.Text = sectionName
            TextBox6.Text = timeInAsStr
            TextBox5.Text = dateStamp

            lastHandledStudentId = decoded
            lastHandledAtUtc = DateTime.UtcNow

        Catch ex As Exception
           ' Error handling
        End Try
    End Sub

    Private Function GetStudent(studentId As String) As StudentScanInfo
        If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
            connect()
        End If

        Const query As String = "SELECT StudentID, Firstname, Middlename, Lastname, Course, Section FROM StudentMasterLists WHERE StudentID = @StudentID"

        Using cmd As New SqliteCommand(query, sqlconn)
            cmd.Parameters.AddWithValue("@StudentID", studentId)
            Using reader = cmd.ExecuteReader()
                If reader.Read() Then
                    Return New StudentScanInfo With {
                        .StudentID = Convert.ToString(reader("StudentID")),
                        .Firstname = Convert.ToString(reader("Firstname")),
                        .Middlename = Convert.ToString(reader("Middlename")),
                        .Lastname = Convert.ToString(reader("Lastname")),
                        .Course = Convert.ToString(reader("Course")),
                        .Section = Convert.ToString(reader("Section"))
                    }
                End If
            End Using
        End Using

        Return Nothing
    End Function

    Private Function ResolveCourseCode(rawCourse As String) As String
        Dim courseText = If(rawCourse, String.Empty).Trim()
        If String.IsNullOrWhiteSpace(courseText) Then
            Return String.Empty
        End If

        If courseText.Contains("-") Then
            Return courseText.Split("-"c)(0).Trim()
        End If

        If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
            connect()
        End If

        Const query As String = "SELECT Code FROM Course WHERE Code = @value OR Name = @value LIMIT 1"
        Using cmd As New SqliteCommand(query, sqlconn)
            cmd.Parameters.AddWithValue("@value", courseText)
            Dim value = cmd.ExecuteScalar()
            If value IsNot Nothing AndAlso value IsNot DBNull.Value Then
                Return Convert.ToString(value)
            End If
        End Using

        Return courseText
    End Function

    Private Function BuildDisplayName(firstname As String, middlename As String, lastname As String) As String
        Dim first = If(firstname, String.Empty).Trim()
        Dim middle = If(middlename, String.Empty).Trim()
        Dim last = If(lastname, String.Empty).Trim()

        Dim middleInitial = String.Empty
        If middle.Length > 0 Then
            middleInitial = " " & Char.ToUpperInvariant(middle(0)) & "."
        End If

        Return (first & middleInitial & " " & last).Trim()
    End Function

    Private Function SaveAttendance(studentId As String) As String
        If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
            connect()
        End If

        Dim now = DateTime.Now
        Dim dateStamp = now.ToString("yyyy-MM-dd")
        Dim timeIn = now.ToString("HH:mm:ss")

        Const insertSql As String = "INSERT INTO Attendance (StudentID, Date_STAMP, TimeIN) VALUES (@StudentID, @Date_STAMP, @TimeIN)"

        Using cmd As New SqliteCommand(insertSql, sqlconn)
            cmd.Parameters.AddWithValue("@StudentID", studentId)
            cmd.Parameters.AddWithValue("@Date_STAMP", dateStamp)
            cmd.Parameters.AddWithValue("@TimeIN", timeIn)
            cmd.ExecuteNonQuery()
        End Using

        Return timeIn
    End Function

    Private Function GetTodayAttendanceTime(studentId As String) As AttendanceScanInfo
        If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
            connect()
        End If

        Const query As String = "SELECT Date_STAMP, TimeIN FROM Attendance WHERE StudentID = @StudentID AND Date_STAMP = @Date_STAMP ORDER BY RecNumber DESC LIMIT 1"

        Using cmd As New SqliteCommand(query, sqlconn)
            cmd.Parameters.AddWithValue("@StudentID", studentId)
            cmd.Parameters.AddWithValue("@Date_STAMP", DateTime.Now.ToString("yyyy-MM-dd"))
            Using reader = cmd.ExecuteReader()
                If reader.Read() Then
                    Return New AttendanceScanInfo With {
                        .DateStamp = Convert.ToString(reader("Date_STAMP")),
                        .TimeIn = Convert.ToString(reader("TimeIN"))
                    }
                End If
            End Using
        End Using

        Return Nothing
    End Function

    Private Sub ShowAttendanceMessage(message As String)
        If isShowingMessage Then
            Return
        End If

        isShowingMessage = True
        'stopCamera()
        MsgBox(message, MsgBoxStyle.Information, "Attendance")
        'startCamera(False)
        isShowingMessage = False
    End Sub

    Sub startCamera(Optional resetState As Boolean = True)
        Try
            stopCamera()
            If resetState Then
                lastDecodeAttemptUtc = DateTime.MinValue
                lastHandledStudentId = String.Empty
                lastHandledAtUtc = DateTime.MinValue
            End If

            camera = New VideoCapture(0)
            If Not camera.IsOpened() Then
                MsgBox("Failed to open camera.", MsgBoxStyle.Exclamation, "System Message:")
                Return
            End If

            frameTimer.Start()
        Catch
            MsgBox("Failed to start camera.", MsgBoxStyle.Exclamation, "System Message:")
        End Try
    End Sub

    Sub stopCamera()
        Try
            If frameTimer IsNot Nothing Then
                frameTimer.Stop()
            End If

            If camera IsNot Nothing Then
                If camera.IsOpened() Then
                    camera.Release()
                End If
                camera.Dispose()
                camera = Nothing
            End If
        Catch
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        stopCamera()
    End Sub

    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        stopCamera()
        latestFrame?.Dispose()
        latestFrame = Nothing
        qrDetector.Dispose()
    End Sub

    Private Class AttendanceScanInfo
        Public Property DateStamp As String
        Public Property TimeIn As String
    End Class

    Private Class StudentScanInfo
        Public Property StudentID As String
        Public Property Firstname As String
        Public Property Middlename As String
        Public Property Lastname As String
        Public Property Course As String
        Public Property Section As String
    End Class
End Class