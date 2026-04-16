Imports Microsoft.Data.Sqlite

Public Class ProfessorRepository
    Public Sub Save(connection As SqliteConnection, professor As Professor)
        Const sql As String = "INSERT INTO Professor (FirstName, MiddleName, LastName) VALUES (@FirstName, @MiddleName, @LastName)"
        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@FirstName", professor.FirstName)
            cmd.Parameters.AddWithValue("@MiddleName", If(professor.MiddleName, DBNull.Value))
            cmd.Parameters.AddWithValue("@LastName", professor.LastName)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub Delete(connection As SqliteConnection, id As Integer)
        Const sql As String = "DELETE FROM Professor WHERE Professor_ID = @id"
        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Class

Public Class CourseRepository
    Public Sub Save(connection As SqliteConnection, course As Course)
        Const sql As String = "INSERT INTO Course (Code, Name) VALUES (@Code, @Name)"
        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@Code", course.Code)
            cmd.Parameters.AddWithValue("@Name", course.Name)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub Delete(connection As SqliteConnection, id As Integer)
        Const sql As String = "DELETE FROM Course WHERE Course_ID = @id"
        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Class

Public Class ClassSectionRepository
    Public Function Save(connection As SqliteConnection, classSection As ClassSection) As Integer
        Const sql As String = "INSERT INTO ClassSection (Course_ID, Professor_ID, SectionName, GracePeriodMinutes) VALUES (@Course_ID, @Professor_ID, @SectionName, @GracePeriodMinutes);" &
                              "SELECT last_insert_rowid();"
        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@Course_ID", classSection.Course_ID)
            cmd.Parameters.AddWithValue("@Professor_ID", classSection.Professor_ID)
            cmd.Parameters.AddWithValue("@SectionName", classSection.SectionName)
            cmd.Parameters.AddWithValue("@GracePeriodMinutes", classSection.GracePeriodMinutes)
            Return Convert.ToInt32(cmd.ExecuteScalar())
        End Using
    End Function

    Public Sub Delete(connection As SqliteConnection, id As Integer)
        Const sql As String = "DELETE FROM ClassSection WHERE ClassSection_ID = @id"
        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Class

Public Class ClassSessionRepository
    Public Sub Save(connection As SqliteConnection, session As ClassSession)
        ' Ensure the time format is consistent HH:mm for string comparisons
        If session.StartTime.Length <> 5 OrElse session.EndTime.Length <> 5 Then
            Throw New ArgumentException("StartTime and EndTime must be in HH:mm format.")
        End If

        ' Verify there is no conflict for the Professor
        Const checkSql As String = "SELECT COUNT(*) FROM ClassSession sess " &
                                   "JOIN ClassSection sec ON sess.ClassSection_ID = sec.ClassSection_ID " &
                                   "WHERE sec.Professor_ID = (SELECT Professor_ID FROM ClassSection WHERE ClassSection_ID = @ClassSection_ID) " &
                                   "AND sess.DayOfWeek = @DayOfWeek " &
                                   "AND sess.StartTime < @EndTime AND sess.EndTime > @StartTime"

        Using checkCmd As New SqliteCommand(checkSql, connection)
            checkCmd.Parameters.AddWithValue("@ClassSection_ID", session.ClassSection_ID)
            checkCmd.Parameters.AddWithValue("@DayOfWeek", session.DayOfWeek)
            checkCmd.Parameters.AddWithValue("@StartTime", session.StartTime)
            checkCmd.Parameters.AddWithValue("@EndTime", session.EndTime)
            Dim conflicts As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
            If conflicts > 0 Then
                Throw New InvalidOperationException("Schedule conflict: The professor is already teaching another class during this time period.")
            End If
        End Using

        Const sql As String = "INSERT INTO ClassSession (ClassSection_ID, DayOfWeek, StartTime, EndTime) VALUES (@ClassSection_ID, @DayOfWeek, @StartTime, @EndTime)"
        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@ClassSection_ID", session.ClassSection_ID)
            cmd.Parameters.AddWithValue("@DayOfWeek", session.DayOfWeek)
            cmd.Parameters.AddWithValue("@StartTime", session.StartTime)
            cmd.Parameters.AddWithValue("@EndTime", session.EndTime)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Sub Delete(connection As SqliteConnection, id As Integer)
        Const sql As String = "DELETE FROM ClassSession WHERE ClassSession_ID = @id"
        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Class

Public Class EnrollmentRepository
    Public Sub Save(connection As SqliteConnection, enrollment As Enrollment)
        Const sql As String = "INSERT INTO Enrollment (ClassSection_ID, Student_ID) VALUES (@ClassSection_ID, @Student_ID)"
        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@ClassSection_ID", enrollment.ClassSection_ID)
            cmd.Parameters.AddWithValue("@Student_ID", enrollment.Student_ID)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Function GetEnrollment(connection As SqliteConnection, studentId As Integer, classSectionId As Integer) As Enrollment
        Const sql As String = "SELECT * FROM Enrollment WHERE Student_ID = @Student_ID AND ClassSection_ID = @ClassSection_ID"
        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@Student_ID", studentId)
            cmd.Parameters.AddWithValue("@ClassSection_ID", classSectionId)
            Using reader = cmd.ExecuteReader()
                If reader.Read() Then
                    Return New Enrollment() With {
                        .Enrollment_ID = Convert.ToInt32(reader("Enrollment_ID")),
                        .ClassSection_ID = Convert.ToInt32(reader("ClassSection_ID")),
                        .Student_ID = Convert.ToInt32(reader("Student_ID"))
                    }
                End If
            End Using
        End Using
        Return Nothing
    End Function
End Class

Public Class AttendanceRepository
    Public Sub RecordAttendance(connection As SqliteConnection, attendance As Attendance)
        Const sql As String = "INSERT INTO Attendance (ClassSession_ID, Enrollment_ID, Date_Stamp, TimeIn, Status) VALUES (@ClassSession_ID, @Enrollment_ID, @Date_Stamp, @TimeIn, @Status)"
        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@ClassSession_ID", attendance.ClassSession_ID)
            cmd.Parameters.AddWithValue("@Enrollment_ID", attendance.Enrollment_ID)
            cmd.Parameters.AddWithValue("@Date_Stamp", attendance.Date_Stamp)
            cmd.Parameters.AddWithValue("@TimeIn", If(attendance.TimeIn, DBNull.Value))
            cmd.Parameters.AddWithValue("@Status", attendance.Status)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Function HasRecord(connection As SqliteConnection, enrollmentId As Integer, sessionId As Integer, dateStamp As String) As Boolean
        Const sql As String = "SELECT COUNT(*) FROM Attendance WHERE Enrollment_ID = @Enrollment_ID AND ClassSession_ID = @ClassSession_ID AND Date_Stamp = @Date_Stamp"
        Using cmd As New SqliteCommand(sql, connection)
            cmd.Parameters.AddWithValue("@Enrollment_ID", enrollmentId)
            cmd.Parameters.AddWithValue("@ClassSession_ID", sessionId)
            cmd.Parameters.AddWithValue("@Date_Stamp", dateStamp)
            Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
        End Using
    End Function
End Class
