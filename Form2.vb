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

            Dim student = GetStudent(decoded)
            If student Is Nothing Then
                Return
            End If

            Dim timeIn As String
            Dim dateStamp As String
            Dim msg As String
            Dim existingAttendance = GetTodayAttendanceTime(decoded)
            If existingAttendance Is Nothing Then
                timeIn = SaveAttendance(decoded)
                dateStamp = DateTime.Now.ToString("yyyy-MM-dd")
                msg = "Attendance" & Environment.NewLine & "Recorded!"
            Else
                timeIn = existingAttendance.TimeIn
                dateStamp = existingAttendance.DateStamp
                msg = "Already" & Environment.NewLine & "Present!"
                'ShowAttendanceMessage("Already timed in on " & dateStamp & " at " & timeIn)
            End If

            TextBox1.Text = student.StudentID
            TextBox2.Text = BuildDisplayName(student.Firstname, student.Middlename, student.Lastname)
            TextBox4.Text = ResolveCourseCode(student.Course)
            TextBox3.Text = student.Section
            TextBox6.Text = timeIn
            TextBox5.Text = dateStamp
            Label7.Text = msg

            lastHandledStudentId = decoded
            lastHandledAtUtc = DateTime.UtcNow
        Catch
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