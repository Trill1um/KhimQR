Public Class PageNavigationManager
    Private ReadOnly host As Panel
    Private currentPage As Form

    Public Sub New(hostPanel As Panel)
        host = hostPanel
    End Sub

    Public Sub Navigate(page As Form)
        If currentPage IsNot Nothing Then
            host.Controls.Remove(currentPage)
            currentPage.Dispose()
        End If

        currentPage = page
        currentPage.TopLevel = False
        currentPage.FormBorderStyle = FormBorderStyle.None
        currentPage.Dock = DockStyle.Fill

        host.Controls.Add(currentPage)
        currentPage.Show()
    End Sub
End Class
