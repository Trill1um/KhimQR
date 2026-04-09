Imports Microsoft.Data.Sqlite

Public Class StudentRepository
    Public Sub Save(connection As SqliteConnection, student As Student)
        Const insertSql As String = "INSERT INTO Student (Student_Code, FirstName, MiddleName, LastName) VALUES (@Student_Code, @FirstName, @MiddleName, @LastName)"

        Using cmd As New SqliteCommand(insertSql, connection)
            cmd.Parameters.AddWithValue("@Student_Code", student.Student_Code)
            cmd.Parameters.AddWithValue("@FirstName", student.FirstName)
            cmd.Parameters.AddWithValue("@MiddleName", student.MiddleName)
            cmd.Parameters.AddWithValue("@LastName", student.LastName)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Function GetByStudentCode(connection As SqliteConnection, studentCode As String) As Student
        Const selectSql As String = "SELECT ID, Student_Code, FirstName, MiddleName, LastName FROM Student WHERE Student_Code = @Student_Code"

        Using cmd As New SqliteCommand(selectSql, connection)
            cmd.Parameters.AddWithValue("@Student_Code", studentCode)
            Using reader = cmd.ExecuteReader()
                If reader.Read() Then
                    Return New Student With {
                        .ID = Convert.ToInt32(reader("ID")),
                        .Student_Code = reader("Student_Code").ToString(),
                        .FirstName = reader("FirstName").ToString(),
                        .MiddleName = If(IsDBNull(reader("MiddleName")), Nothing, reader("MiddleName").ToString()),
                        .LastName = reader("LastName").ToString()
                    }
                End If
            End Using
        End Using
        Return Nothing
    End Function
End Class

