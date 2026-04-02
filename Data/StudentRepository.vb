Imports Microsoft.Data.Sqlite

Public Class StudentRepository
    Public Sub Save(connection As SqliteConnection, student As StudentRecord, qrCodeBytes As Byte())
        Const insertSql As String = "INSERT INTO StudentMasterLists (StudentID, Firstname, Middlename, Lastname, Course, Section, QRCode) VALUES (@StudentID, @Firstname, @Middlename, @Lastname, @Course, @Section, @QRCode)"

        Using cmd As New SqliteCommand(insertSql, connection)
            cmd.Parameters.AddWithValue("@StudentID", student.StudentID)
            cmd.Parameters.AddWithValue("@Firstname", student.Firstname)
            cmd.Parameters.AddWithValue("@Middlename", student.Middlename)
            cmd.Parameters.AddWithValue("@Lastname", student.Lastname)
            cmd.Parameters.AddWithValue("@Course", student.Course)
            cmd.Parameters.AddWithValue("@Section", student.Section)
            cmd.Parameters.AddWithValue("@QRCode", qrCodeBytes)
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Class

