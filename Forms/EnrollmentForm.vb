Imports Microsoft.Data.Sqlite

Partial Public Class EnrollmentForm
    Private ReadOnly studentRepository As New StudentRepository()
    Private ReadOnly enrollmentRepository As New EnrollmentRepository()

    Private currentStudent As Student

    Private Sub EnrollmentForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connect()
        LoadCourses()
        ClearStudentDisplay()
    End Sub

    Private Sub LoadCourses()
        Dim table As New DataTable()
        Using cmd As New SqliteCommand("SELECT Course_ID, Code || ' - ' || Name AS DisplayText FROM Course ORDER BY Code", sqlconn)
            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using

        ComboBox1.DataSource = table
        ComboBox1.DisplayMember = "DisplayText"
        ComboBox1.ValueMember = "Course_ID"

        LoadSectionsForSelectedCourse()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        LoadSectionsForSelectedCourse()
    End Sub

    Private Sub LoadSectionsForSelectedCourse()
        ComboBox2.DataSource = Nothing

        If ComboBox1.SelectedValue Is Nothing Then
            Return
        End If

        Dim courseId As Integer
        If Not Integer.TryParse(ComboBox1.SelectedValue.ToString(), courseId) Then
            Return
        End If

        Dim table As New DataTable()

        Using cmd As New SqliteCommand("SELECT ClassSection_ID, SectionName AS DisplayText FROM ClassSection WHERE Course_ID = @c ORDER BY SectionName", sqlconn)
            cmd.Parameters.AddWithValue("@c", courseId)
            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using

        ComboBox2.DataSource = table
        ComboBox2.DisplayMember = "DisplayText"
        ComboBox2.ValueMember = "ClassSection_ID"
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode <> Keys.Enter Then Return

        e.SuppressKeyPress = True
        LookupStudent()
    End Sub

    Private Sub LookupStudent()
        Dim studentCode = TextBox1.Text.Trim()
        If String.IsNullOrWhiteSpace(studentCode) Then
            ClearStudentDisplay()
            Return
        End If

        currentStudent = studentRepository.GetByStudentCode(sqlconn, studentCode)
        If currentStudent Is Nothing Then
            MsgBox("Student not found", MsgBoxStyle.Exclamation, "Enrollment")
            TextBox1.Clear()
            ClearStudentDisplay()
            TextBox1.Focus()
            Return
        End If

        TextBox2.Text = currentStudent.FirstName
        TextBox4.Text = currentStudent.MiddleName
        TextBox3.Text = currentStudent.LastName
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            If currentStudent Is Nothing Then
                Throw New InvalidOperationException("Please enter a valid Student ID and press Enter first.")
            End If

            If ComboBox2.SelectedValue Is Nothing Then
                Throw New InvalidOperationException("Please select a section.")
            End If

            Dim classSectionId = Convert.ToInt32(ComboBox2.SelectedValue)

            enrollmentRepository.Save(sqlconn, New Enrollment With {
                .Student_ID = currentStudent.ID,
                .ClassSection_ID = classSectionId,
                .EnrollmentDate = SystemClock.Today.ToString("yyyy-MM-dd")
            })

            MsgBox("Student enrolled successfully.", MsgBoxStyle.Information, "Enrollment")
            ClearForNext()
        Catch ex As SqliteException When ex.SqliteErrorCode = 19
            MsgBox("Student is already enrolled in this section.", MsgBoxStyle.Information, "Enrollment")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Enrollment")
        End Try
    End Sub

    Private Sub ClearStudentDisplay()
        currentStudent = Nothing
        TextBox2.Clear()
        TextBox4.Clear()
        TextBox3.Clear()
    End Sub

    Private Sub ClearForNext()
        TextBox1.Clear()
        ClearStudentDisplay()
        TextBox1.Focus()
    End Sub
End Class
