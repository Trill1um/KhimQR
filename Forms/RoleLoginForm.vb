Imports Microsoft.Data.Sqlite

Public Class RoleLoginForm
    Public Event LoginSucceeded As EventHandler(Of RoleLoginEventArgs)

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub RoleLoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadLoginChoices()
    End Sub

    Private Sub LoadLoginChoices()
        If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
            connect()
        End If

        Dim choices As New DataTable()
        choices.Columns.Add("DisplayText", GetType(String))
        choices.Columns.Add("IsAdmin", GetType(Boolean))
        choices.Columns.Add("Professor_ID", GetType(Integer))

        choices.Rows.Add("Admin", True, DBNull.Value)

        Const sql As String = "SELECT Professor_ID, FirstName || ' ' || LastName AS FullName FROM Professor ORDER BY LastName, FirstName"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    choices.Rows.Add(reader("FullName").ToString(), False, Convert.ToInt32(reader("Professor_ID")))
                End While
            End Using
        End Using

        roleComboBox.DataSource = choices
        roleComboBox.DisplayMember = "DisplayText"
        roleComboBox.ValueMember = "Professor_ID"

        If roleComboBox.Items.Count > 0 Then
            roleComboBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub loginButton_Click(sender As Object, e As EventArgs) Handles loginButton.Click
        Dim selected = TryCast(roleComboBox.SelectedItem, DataRowView)
        If selected Is Nothing Then
            MessageBox.Show("Please select a login option.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim isAdmin = Convert.ToBoolean(selected("IsAdmin"))
        Dim displayName = selected("DisplayText").ToString()

        Dim professorId As Integer? = Nothing
        If Not isAdmin AndAlso selected("Professor_ID") IsNot DBNull.Value Then
            professorId = Convert.ToInt32(selected("Professor_ID"))
        End If

        RaiseEvent LoginSucceeded(Me, New RoleLoginEventArgs(isAdmin, professorId, displayName))
    End Sub
End Class