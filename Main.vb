Public Class Main
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AdminForm.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Using f As New AddForm()
        '    f.ShowDialog(Me)
        'End Using
        AddForm.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Using loginForm As New ProfessorLoginForm()
            If loginForm.ShowDialog(Me) = DialogResult.OK Then
                Using f As New Form2()
                    f.CurrentProfessorId = loginForm.SelectedProfessorId
                    f.ShowDialog(Me)
                End Using
            End If
        End Using
    End Sub
End Class