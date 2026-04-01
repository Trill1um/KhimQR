Imports Microsoft.Data.SqlClient
Imports QRCoder

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connect()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim input = TextBox1.Text
            Using qrGenerator As New QRCodeGenerator()
                Using qrCodeData = qrGenerator.CreateQrCode(input, QRCodeGenerator.ECCLevel.Q)
                    Using qrCode As New QRCode(qrCodeData)
                        PictureBox1.Image = qrCode.GetGraphic(20)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            PictureBox1.Image = Nothing
        End Try
    End Sub
End Class
