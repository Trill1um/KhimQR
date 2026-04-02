Imports QRCoder
Imports System.IO

Public Class Form1
    Private ReadOnly qrCodeService As New QrCodeService()
    Private ReadOnly qrStorageService As New QrStorageService()
    Private ReadOnly autoNumberRepository As New AutoNumberRepository()
    Private ReadOnly studentRepository As New StudentRepository()

    Private Const DefaultPrefix As String = "MAR"
    Private Const AutoNumberWidth As Integer = 4

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button2.Image = SystemIcons.GetStockIcon(StockIconId.Folder).ToBitmap()
        connect()
        qrStorageService.EnsureDefaultDirectory()

        If String.IsNullOrWhiteSpace(TextBox6.Text) Then
            TextBox6.Text = DefaultPrefix
        End If

        AutoNumber()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim input = GetFullStudentId()

            If String.IsNullOrWhiteSpace(input) Then
                PictureBox1.Image = Nothing
                Return
            End If

            PictureBox1.Image = qrCodeService.Generate(input, 20)
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

        Dim student As New StudentRecord With {
            .StudentID = GetFullStudentId(),
            .Firstname = TextBox2.Text,
            .Middlename = TextBox3.Text,
            .Lastname = TextBox4.Text,
            .Course = ComboBox1.Text,
            .Section = TextBox5.Text
        }

        Using ms As New MemoryStream()
            PictureBox1.Image.Save(ms, Imaging.ImageFormat.Png)
            studentRepository.Save(sqlconn, student, ms.ToArray())
        End Using

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

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Using admin As New AdminForm()
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
