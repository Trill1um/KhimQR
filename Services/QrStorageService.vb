Imports System.Drawing.Imaging
Imports System.IO

Public Class QrStorageService
    Public Function GetDefaultDirectory() As String
        Return Path.Combine(Application.StartupPath, "Storage", "QrCodes")
    End Function

    Public Sub EnsureDefaultDirectory()
        Directory.CreateDirectory(GetDefaultDirectory())
    End Sub

    Public Function SaveWithDialog(image As Image, suggestedFileName As String) As String
        EnsureDefaultDirectory()

        Using sd As New SaveFileDialog()
            sd.InitialDirectory = GetDefaultDirectory()
            sd.FileName = GetSafeFileName(suggestedFileName)
            sd.Filter = "PNG|*.png"

            If sd.ShowDialog() = DialogResult.OK Then
                image.Save(sd.FileName, ImageFormat.Png)
                Return sd.FileName
            End If
        End Using

        Return String.Empty
    End Function

    Public Function SaveToDefaultDirectory(image As Image, suggestedFileName As String) As String
        EnsureDefaultDirectory()

        Dim fullPath = Path.Combine(GetDefaultDirectory(), GetSafeFileName(suggestedFileName) & ".png")
        image.Save(fullPath, ImageFormat.Png)
        Return fullPath
    End Function

    Private Function GetSafeFileName(fileName As String) As String
        If String.IsNullOrWhiteSpace(fileName) Then
            Return "QRCode"
        End If

        Dim safeName = fileName
        For Each invalidChar In Path.GetInvalidFileNameChars()
            safeName = safeName.Replace(invalidChar, "_"c)
        Next

        Return safeName
    End Function
End Class
