Imports Microsoft.Data.Sqlite
Imports OpenCvSharp
Imports OpenCvSharp.Extensions

Public Class Form2
    Private camera As VideoCapture
    Private frameTimer As Timer
    Private latestFrame As Mat
    Private ReadOnly qrDetector As New QRCodeDetector()
    Private lastDecodeAttemptUtc As DateTime = DateTime.MinValue
    Private scanHandled As Boolean

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        frameTimer = New Timer() With {.Interval = 33}
        AddHandler frameTimer.Tick, AddressOf FrameTimer_Tick
    End Sub

    Private Sub FrameTimer_Tick(sender As Object, e As EventArgs)
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

            If Not scanHandled AndAlso (DateTime.UtcNow - lastDecodeAttemptUtc).TotalMilliseconds >= 200 Then
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

            If Not String.IsNullOrWhiteSpace(decoded) Then
                scanHandled = True
                TextBox1.Text = decoded
                SaveAttendance(decoded)
                stopCamera()
                MsgBox(decoded, MsgBoxStyle.Information, "Scanned QR Data")
            End If
        Catch
        End Try
    End Sub

    Private Sub SaveAttendance(studentId As String)
        If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
            connect()
        End If

        Const insertSql As String = "INSERT INTO Attendance (StudentID, Date_STAMP, TimeIN) VALUES (@StudentID, @Date_STAMP, @TimeIN)"

        Using cmd As New SqliteCommand(insertSql, sqlconn)
            cmd.Parameters.AddWithValue("@StudentID", studentId)
            cmd.Parameters.AddWithValue("@Date_STAMP", DateTime.Now.ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@TimeIN", DateTime.Now.ToString("HH:mm:ss"))
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Sub startCamera()
        Try
            stopCamera()
            scanHandled = False
            lastDecodeAttemptUtc = DateTime.MinValue

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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        startCamera()
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
End Class