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

    Sub AutoNumber()
        Const prefix As String = "MAR"
        str = "SELECT MAX(NewNumber) FROM Autonumber WHERE pfx = @pfx"

        Dim currentNumber As Object
        Using autoCmd As New SqlCommand(str, sqlconn)
            autoCmd.Parameters.AddWithValue("@pfx", prefix)
            currentNumber = autoCmd.ExecuteScalar()
        End Using

        If currentNumber Is Nothing OrElse currentNumber Is DBNull.Value Then
            CreateNewAutoNumber()

            Using refreshCmd As New SqlCommand(str, sqlconn)
                refreshCmd.Parameters.AddWithValue("@pfx", prefix)
                Dim generatedNumber = refreshCmd.ExecuteScalar()
                TextBox1.Text = If(generatedNumber Is Nothing OrElse generatedNumber Is DBNull.Value, String.Empty, Convert.ToString(generatedNumber))
            End Using
        Else
            TextBox1.Text = Convert.ToString(currentNumber)
        End If
    End Sub

    Sub CreateNewAutoNumber()
        Try
            Using autoNoCmd As New SqlCommand("SP_AutoNo_AMS", sqlconn)
                autoNoCmd.CommandType = CommandType.StoredProcedure
                autoNoCmd.Parameters.AddWithValue("@pfx", "MAR")
                autoNoCmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class
