Imports QRCoder
Imports System.IO
Imports Microsoft.Data.Sqlite

Partial Public Class Form1
    Private ReadOnly qrCodeService As New QrCodeService()
    Private ReadOnly qrStorageService As New QrStorageService()
    Private ReadOnly studentRepository As New StudentRepository()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connect()
        qrStorageService.EnsureDefaultDirectory()
        LoadCourses()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        GenerateQrPreview()
    End Sub

    Private Sub GenerateQrPreview()
        Try
            Dim size = Math.Min(PictureBox1.Width, PictureBox1.Height)
            Dim pixelsPerModule = Math.Max(1, size \ 25)
            Dim input = GetFullStudentCode()

            If String.IsNullOrWhiteSpace(input) Then
                PictureBox1.Image = Nothing
                Return
            End If

            PictureBox1.Image = qrCodeService.Generate(input, pixelsPerModule)
        Catch
            PictureBox1.Image = Nothing
        End Try
    End Sub

    Sub LoadCourses()
        ComboBox1.Items.Clear()
        Using cmd As New SqliteCommand("SELECT Code || ' - ' || Name FROM Course ORDER BY Code", sqlconn)
            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    ComboBox1.Items.Add(reader.GetString(0))
                End While
            End Using
        End Using
    End Sub

    Private Function GetFullStudentCode() As String
        Return TextBox1.Text.Trim()
    End Function

    Private Function isQrGenerated() As Boolean
        If PictureBox1.Image Is Nothing Then
            MsgBox("Generate QR code first.", MsgBoxStyle.Exclamation, "System Message:")
            Return False
        End If
        Return True
    End Function

    Sub savestudentdata()
        If Not isQrGenerated() Then
            Return
        End If

        Dim studentCode = GetFullStudentCode()
        Dim sectionInput As String = TextBox5.Text.Trim()
        Dim courseId As Integer = 0

        If String.IsNullOrWhiteSpace(studentCode) Then
            Throw New InvalidOperationException("Please enter a Student Code.")
        End If

        If ComboBox1.SelectedItem Is Nothing OrElse String.IsNullOrWhiteSpace(sectionInput) Then
            Throw New InvalidOperationException("Please select a Course and provide a Section.")
        End If

        Dim selectedCourse As String = ComboBox1.SelectedItem.ToString()
        Dim courseCode As String = selectedCourse.Split("-"c)(0).Trim()

        Using cmd As New SqliteCommand("SELECT Course_ID FROM Course WHERE Code = @c", sqlconn)
            cmd.Parameters.AddWithValue("@c", courseCode)
            Dim res = cmd.ExecuteScalar()
            If res IsNot Nothing AndAlso Not DBNull.Value.Equals(res) Then
                courseId = Convert.ToInt32(res)
            Else
                Throw New InvalidOperationException($"The selected course '{courseCode}' does not exist in the database.")
            End If
        End Using

        Dim parsedSectionId As Integer
        If Integer.TryParse(sectionInput, parsedSectionId) Then
            SaveStudentData(courseId, parsedSectionId, studentCode)
        Else
            SaveStudentData(courseId, sectionInput, studentCode)
        End If

        qrStorageService.SaveToDefaultDirectory(PictureBox1.Image, studentCode)
    End Sub

    Private Sub SaveStudentData(courseId As Integer, sectionId As Integer, studentCode As String)
        Dim classSectionId As Integer = 0

        Using cmd As New SqliteCommand("SELECT ClassSection_ID FROM ClassSection WHERE Course_ID = @c AND Section_ID = @sid", sqlconn)
            cmd.Parameters.AddWithValue("@c", courseId)
            cmd.Parameters.AddWithValue("@sid", sectionId)
            Dim res = cmd.ExecuteScalar()
            If res IsNot Nothing AndAlso Not DBNull.Value.Equals(res) Then
                classSectionId = Convert.ToInt32(res)
            Else
                Throw New InvalidOperationException($"The class section ID '{sectionId}' for the selected course does not exist. Please create this section in the database.")
            End If
        End Using

        SaveStudentAndEnroll(studentCode, classSectionId)
    End Sub

    Private Sub SaveStudentData(courseId As Integer, sectionName As String, studentCode As String)
        Dim classSectionId As Integer = 0

        Using cmd As New SqliteCommand("SELECT ClassSection_ID FROM ClassSection WHERE Course_ID = @c AND SectionName = @s", sqlconn)
            cmd.Parameters.AddWithValue("@c", courseId)
            cmd.Parameters.AddWithValue("@s", sectionName)
            Dim res = cmd.ExecuteScalar()
            If res IsNot Nothing AndAlso Not DBNull.Value.Equals(res) Then
                classSectionId = Convert.ToInt32(res)
            Else
                Throw New InvalidOperationException($"The class section '{sectionName}' for the selected course does not exist. Please create this section in the database.")
            End If
        End Using

        SaveStudentAndEnroll(studentCode, classSectionId)
    End Sub

    Private Sub SaveStudentAndEnroll(studentCode As String, classSectionId As Integer)
        Using tx = sqlconn.BeginTransaction()
            Dim student = studentRepository.GetByStudentCode(sqlconn, studentCode)

            If student Is Nothing Then
                student = New Student With {
                    .Student_Code = studentCode,
                    .FirstName = TextBox2.Text,
                    .MiddleName = TextBox4.Text,
                    .LastName = TextBox3.Text
                }

                Const insertStudentSql As String = "INSERT INTO Student (Student_Code, FirstName, MiddleName, LastName) VALUES (@Student_Code, @FirstName, @MiddleName, @LastName)"
                Using cmd As New SqliteCommand(insertStudentSql, sqlconn, tx)
                    cmd.Parameters.AddWithValue("@Student_Code", student.Student_Code)
                    cmd.Parameters.AddWithValue("@FirstName", student.FirstName)
                    cmd.Parameters.AddWithValue("@MiddleName", If(String.IsNullOrWhiteSpace(student.MiddleName), DBNull.Value, student.MiddleName))
                    cmd.Parameters.AddWithValue("@LastName", student.LastName)
                    cmd.ExecuteNonQuery()
                End Using

                student = studentRepository.GetByStudentCode(sqlconn, studentCode)
            End If

            Const enrollSql As String = "INSERT OR IGNORE INTO Enrollment (ClassSection_ID, Student_ID) VALUES (@ClassSection_ID, @Student_ID)"
            Using cmd As New SqliteCommand(enrollSql, sqlconn, tx)
                cmd.Parameters.AddWithValue("@ClassSection_ID", classSectionId)
                cmd.Parameters.AddWithValue("@Student_ID", student.ID)
                cmd.ExecuteNonQuery()
            End Using

            tx.Commit()
        End Using
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            savestudentdata()
            clear()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "System Message:")
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Using admin As New AdminForm
            admin.ShowDialog(Me)
        End Using
    End Sub

    Sub clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox1.Clear()
        PictureBox1.Image = Nothing
    End Sub

End Class
