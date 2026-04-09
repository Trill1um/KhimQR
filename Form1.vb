Imports QRCoder
Imports System.IO
Imports Microsoft.Data.Sqlite

Public Class Form1
    Private ReadOnly qrCodeService As New QrCodeService()
    Private ReadOnly qrStorageService As New QrStorageService()
    Private ReadOnly studentRepository As New StudentRepository()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connect()
        qrStorageService.EnsureDefaultDirectory()
        LoadCourses()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim size = Math.Min(PictureBox1.Width, PictureBox1.Height)
            Dim pixelsPerModule = Math.Max(1, size \ 25)
            Dim input = GetFullStudentId()

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

    Private Function GetFullStudentId() As String
        Return TextBox1.Text.Trim()
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If PictureBox1.Image Is Nothing Then
            MsgBox("Generate QR code first.", MsgBoxStyle.Exclamation, "System Message:")
            Return
        End If

        qrStorageService.SaveWithDialog(PictureBox1.Image, GetFullStudentId())
    End Sub

    Sub savestudentdata()
        If PictureBox1.Image Is Nothing Then
            Throw New InvalidOperationException("Generate QR code first.")
        End If

        Dim student As New Student With {
            .Student_Code = GetFullStudentId(),
            .FirstName = TextBox2.Text,
            .MiddleName = TextBox4.Text,
            .LastName = TextBox3.Text
        }

        studentRepository.Save(sqlconn, student)

        ' Reload student to get ID
        student = studentRepository.GetByStudentCode(sqlconn, student.Student_Code)

        ' Try Enrolling
        If ComboBox1.SelectedItem IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(TextBox5.Text) Then
            Dim selectedCourse As String = ComboBox1.SelectedItem.ToString()
            Dim courseCode As String = selectedCourse.Split("-"c)(0).Trim()
            Dim sectionName As String = TextBox5.Text.Trim()

            Dim courseId As Integer = 0
            Using cmd As New SqliteCommand("SELECT Course_ID FROM Course WHERE Code = @c", sqlconn)
                cmd.Parameters.AddWithValue("@c", courseCode)
                Dim res = cmd.ExecuteScalar()
                If res IsNot Nothing AndAlso Not DBNull.Value.Equals(res) Then
                    courseId = Convert.ToInt32(res)
                End If
            End Using

            If courseId > 0 Then
                Dim classSectionId As Integer = 0
                Using cmd As New SqliteCommand("SELECT ClassSection_ID FROM ClassSection WHERE Course_ID = @c AND SectionName = @s", sqlconn)
                    cmd.Parameters.AddWithValue("@c", courseId)
                    cmd.Parameters.AddWithValue("@s", sectionName)
                    Dim res = cmd.ExecuteScalar()
                    If res IsNot Nothing AndAlso Not DBNull.Value.Equals(res) Then
                        classSectionId = Convert.ToInt32(res)
                    End If
                End Using

                If classSectionId = 0 Then
                    Dim profId As Integer = 1
                    Using cmd As New SqliteCommand("SELECT Professor_ID FROM Professor LIMIT 1", sqlconn)
                        Dim res = cmd.ExecuteScalar()
                        If res IsNot Nothing Then profId = Convert.ToInt32(res)
                    End Using

                    Dim repo As New ClassSectionRepository()
                    classSectionId = repo.Save(sqlconn, New ClassSection With {
                        .Course_ID = courseId,
                        .Professor_ID = profId,
                        .SectionName = sectionName,
                        .GracePeriodMinutes = 15
                    })

                    ' Add default all-day session for testing
                    Dim sessionRepo As New ClassSessionRepository()
                    For i As Integer = 0 To 6
                        sessionRepo.Save(sqlconn, New ClassSession With {
                            .ClassSection_ID = classSectionId,
                            .DayOfWeek = i,
                            .StartTime = "00:00",
                            .EndTime = "23:59"
                        })
                    Next
                End If

                Dim enrolRepo As New EnrollmentRepository()
                enrolRepo.Save(sqlconn, New Enrollment With {
                    .Student_ID = student.ID,
                    .ClassSection_ID = classSectionId
                })
            End If
        End If

        qrStorageService.SaveToDefaultDirectory(PictureBox1.Image, GetFullStudentId())
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
