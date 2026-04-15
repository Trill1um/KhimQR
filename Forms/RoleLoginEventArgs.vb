Public Class RoleLoginEventArgs
    Inherits EventArgs

    Public Sub New(isAdmin As Boolean, selectedProfessorId As Integer?, selectedDisplayName As String)
        Me.IsAdmin = isAdmin
        Me.SelectedProfessorId = selectedProfessorId
        Me.SelectedDisplayName = selectedDisplayName
        Me.SelectedRole = If(isAdmin, "Admin", "Professor")
    End Sub

    Public ReadOnly Property IsAdmin As Boolean
    Public ReadOnly Property SelectedProfessorId As Integer?
    Public ReadOnly Property SelectedDisplayName As String
    Public ReadOnly Property SelectedRole As String
End Class