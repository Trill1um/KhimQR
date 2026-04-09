Imports QRCoder
Imports System.IO
Imports Microsoft.Data.Sqlite

Public Class Form1
    Private ReadOnly qrCodeService As New QrCodeService()
    Private ReadOnly qrStorageService As New QrStorageService()
    Private ReadOnly autoNumberRepository As New AutoNumberRepository()
    Private ReadOnly studentRepository As New StudentRepository()

    Private Const DefaultPrefix As String = "MAR"
    Private Const AutoNumberWidth As Integer = 4

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connect()
        qrStorageService.EnsureDefaultDirectory()
        SeedCourses()
        LoadCourses()

        If String.IsNullOrWhiteSpace(TextBox6.Text) Then
            TextBox6.Text = DefaultPrefix
        End If

        AutoNumber()
    End Sub

    Sub SeedCourses() 'called once
        Dim courses As New List(Of (Code As String, Name As String)) From {
        ("BSIT", "Bachelor of Science in Information Technology"),
        ("BSCS", "Bachelor of Science in Computer Science"),
        ("BSBA", "Bachelor of Science in Business Administration"),
        ("BSED", "Bachelor of Science in Education"),
        ("BSME", "Bachelor of Science in Mechanical Engineering")
    }

        For Each course In courses
            Using cmd As New SqliteCommand(
            "INSERT OR IGNORE INTO Course (Code, Name) VALUES (@code, @name)", sqlconn)
                cmd.Parameters.AddWithValue("@code", course.Code)
                cmd.Parameters.AddWithValue("@name", course.Name)
                cmd.ExecuteNonQuery()
            End Using
        Next
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

    Sub AutoNumber()
        Dim prefix = GetPrefix()
        Dim code = autoNumberRepository.GetCurrentOrDefault(sqlconn, prefix, AutoNumberWidth)
        TextBox1.Text = GetNumberPart(code, prefix)
    End Sub

    Sub CreateNewAutoNumber()
        Dim prefix = GetPrefix()
        Dim nextCode = autoNumberRepository.IncrementAndGet(sqlconn, prefix, AutoNumberWidth)
        TextBox1.Text = GetNumberPart(nextCode, prefix)
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

    Private Function GetPrefix() As String
        Dim source = TextBox6.Text.Trim()
        If String.IsNullOrWhiteSpace(source) Then
            Return DefaultPrefix
        End If

        Dim chars As New List(Of Char)()
        For Each c In source
            If Char.IsLetterOrDigit(c) Then
                chars.Add(Char.ToUpperInvariant(c))
            End If

            If chars.Count = 6 Then
                Exit For
            End If
        Next

        Dim prefix = New String(chars.ToArray())
        Return If(String.IsNullOrWhiteSpace(prefix), DefaultPrefix, prefix)
    End Function

    Private Function GetNumberPart(code As String, prefix As String) As String
        Dim numberPart = code
        If numberPart.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) Then
            numberPart = numberPart.Substring(prefix.Length)
        End If

        Return numberPart.TrimStart("-"c)
    End Function

    Private Function GetFullStudentId() As String
        Dim prefix = GetPrefix()
        Dim numberPart = TextBox1.Text.Trim()

        If String.IsNullOrWhiteSpace(numberPart) Then
            Return String.Empty
        End If

        Return prefix & numberPart
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

        qrStorageService.SaveToDefaultDirectory(PictureBox1.Image, GetFullStudentId())
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            savestudentdata()
            CreateNewAutoNumber()
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

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged
        If sqlconn IsNot Nothing AndAlso sqlconn.State = ConnectionState.Open Then
            AutoNumber()
        End If
    End Sub

    Sub clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        ComboBox1.Text = Nothing
        PictureBox1.Image = Nothing
    End Sub

End Class
