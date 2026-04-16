Imports Microsoft.Data.Sqlite
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports System.Threading.Tasks

Public Module AttendanceAutoAbsentService
    Public Sub QueueFinalizeProfessorAbsences(professorId As Integer, attendanceDate As DateTime, Optional courseId As Integer? = Nothing, Optional classSessionId As Integer? = Nothing)
        Task.Run(Sub()
                     Try
                         FinalizeProfessorAbsences(professorId, attendanceDate, courseId, classSessionId)
                     Catch
                     End Try
                 End Sub)
    End Sub

    Public Function FinalizeProfessorAbsences(professorId As Integer, attendanceDate As DateTime, Optional courseId As Integer? = Nothing, Optional classSessionId As Integer? = Nothing) As Integer
        If professorId <= 0 Then
            Return 0
        End If

        Dim today = SystemClock.Now.Date
        If attendanceDate.Date > today Then
            Return 0
        End If

        Dim connectionString = $"Data Source={dbPath};Mode=ReadWriteCreate;Cache=Shared"
        Using connection As New SqliteConnection(connectionString)
            connection.Open()
            Return FinalizeProfessorAbsences(connection, professorId, attendanceDate.Date, courseId, classSessionId)
        End Using
    End Function

    Private Function FinalizeProfessorAbsences(connection As SqliteConnection, professorId As Integer, attendanceDate As DateTime, Optional courseId As Integer? = Nothing, Optional classSessionId As Integer? = Nothing) As Integer
        Dim targetSessionIds = GetFinishedSessionIds(connection, professorId, attendanceDate, courseId, classSessionId)
        Dim inserted As Integer = 0

        For Each sessionId In targetSessionIds
            inserted += FinalizeSessionAbsences(connection, sessionId, attendanceDate)
        Next

        Return inserted
    End Function

    Private Function GetFinishedSessionIds(connection As SqliteConnection, professorId As Integer, attendanceDate As DateTime, Optional courseId As Integer? = Nothing, Optional classSessionId As Integer? = Nothing) As List(Of Integer)
        Dim result As New List(Of Integer)()
        Dim today = SystemClock.Now.Date

        If attendanceDate.Date > today Then
            Return result
        End If

        Dim sql As String = "SELECT sess.ClassSession_ID FROM ClassSession sess " &
                            "JOIN ClassSection cs ON sess.ClassSection_ID = cs.ClassSection_ID " &
                            "WHERE cs.Professor_ID = @profId AND sess.DayOfWeek = @dow"

        If courseId.HasValue Then
            sql &= " AND cs.Course_ID = @courseId"
        End If

        If classSessionId.HasValue Then
            sql &= " AND sess.ClassSession_ID = @classSessionId"
        End If

        If attendanceDate.Date = today Then
            sql &= " AND sess.EndTime <= @currentTime"
        End If

        sql &= " ORDER BY sess.ClassSession_ID"

        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@profId", professorId)
            cmd.Parameters.AddWithValue("@dow", CInt(attendanceDate.DayOfWeek))

            If courseId.HasValue Then
                cmd.Parameters.AddWithValue("@courseId", courseId.Value)
            End If

            If classSessionId.HasValue Then
                cmd.Parameters.AddWithValue("@classSessionId", classSessionId.Value)
            End If

            If attendanceDate.Date = today Then
                cmd.Parameters.AddWithValue("@currentTime", SystemClock.Now.ToString("HH:mm"))
            End If

            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    result.Add(Convert.ToInt32(reader("ClassSession_ID")))
                End While
            End Using
        End Using

        Return result
    End Function

    Private Function GetExpectedEnrollmentIds(connection As SqliteConnection, classSessionId As Integer, attendanceDate As DateTime) As List(Of Integer)
        Dim result As New List(Of Integer)()
        Const sql As String = "SELECT e.Student_ID FROM Enrollment e JOIN ClassSection cs ON e.ClassSection_ID = cs.ClassSection_ID JOIN ClassSession sess ON sess.ClassSection_ID = cs.ClassSection_ID WHERE sess.ClassSession_ID = @ClassSession_ID AND e.EnrollmentDate <= @Date_Stamp"

        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@ClassSession_ID", classSessionId)
            cmd.Parameters.AddWithValue("@Date_Stamp", attendanceDate.ToString("yyyy-MM-dd"))
            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    result.Add(Convert.ToInt32(reader("Student_ID")))
                End While
            End Using
        End Using

        Return result
    End Function

    Private Function FinalizeSessionAbsences(connection As SqliteConnection, classSessionId As Integer, attendanceDate As DateTime) As Integer
        Dim dateStamp = attendanceDate.ToString("yyyy-MM-dd")
        Dim expectedEnrollmentIds = GetExpectedEnrollmentIds(connection, classSessionId, attendanceDate)
        If expectedEnrollmentIds.Count = 0 Then
            Return 0
        End If

        Dim attendedEnrollmentIds = GetAttendedEnrollmentIds(connection, classSessionId, dateStamp)
        If attendedEnrollmentIds.Count >= expectedEnrollmentIds.Count Then
            Return 0
        End If

        Dim missingEnrollmentIds = expectedEnrollmentIds.Except(attendedEnrollmentIds).ToList()
        If missingEnrollmentIds.Count = 0 Then
            Return 0
        End If

        Const insertSql As String = "INSERT OR IGNORE INTO Attendance (ClassSession_ID, Enrollment_ID, Date_Stamp, TimeIn, Status) VALUES (@ClassSession_ID, @Enrollment_ID, @Date_Stamp, NULL, 'Absent')"
        Using cmd As New SqliteCommand(insertSql, connection)
            Dim sessionParam = cmd.Parameters.Add("@ClassSession_ID", DbType.Int32)
            Dim enrollmentParam = cmd.Parameters.Add("@Enrollment_ID", DbType.Int32)
            Dim dateParam = cmd.Parameters.Add("@Date_Stamp", DbType.String)

            dateParam.Value = dateStamp

            For Each enrollmentId In missingEnrollmentIds
                sessionParam.Value = classSessionId
                enrollmentParam.Value = enrollmentId
                cmd.ExecuteNonQuery()
            Next
        End Using

        Return missingEnrollmentIds.Count
    End Function

    Private Function GetAttendedEnrollmentIds(connection As SqliteConnection, classSessionId As Integer, dateStamp As String) As List(Of Integer)
        Dim result As New List(Of Integer)()
        Const sql As String = "SELECT DISTINCT Enrollment_ID FROM Attendance WHERE ClassSession_ID = @ClassSession_ID AND Date_Stamp = @Date_Stamp"

        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@ClassSession_ID", classSessionId)
            cmd.Parameters.AddWithValue("@Date_Stamp", dateStamp)
            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    result.Add(Convert.ToInt32(reader("Enrollment_ID")))
                End While
            End Using
        End Using

        Return result
    End Function
End Module
