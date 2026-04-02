Imports QRCoder

Public Class QrCodeService
    Public Function Generate(content As String, pixelsPerModule As Integer) As Bitmap
        Using qrGenerator As New QRCodeGenerator()
            Using qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q)
                Using qrCode As New QRCode(qrCodeData)
                    Return qrCode.GetGraphic(pixelsPerModule)
                End Using
            End Using
        End Using
    End Function
End Class
