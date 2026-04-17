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
        choices.Columns.Add("Password", GetType(String))

        Const sql As String = "SELECT Professor_ID, CASE WHEN IsAdmin = 1 THEN 'Admin' ELSE FirstName || ' ' || CASE WHEN IFNULL(TRIM(MiddleName), '') = '' THEN '' ELSE SUBSTR(TRIM(MiddleName), 1, 1) || '. ' END || LastName END AS DisplayText, Password, IsAdmin FROM Professor ORDER BY IsAdmin DESC, LastName, FirstName"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    choices.Rows.Add(reader("DisplayText").ToString(), Convert.ToBoolean(reader("IsAdmin")), Convert.ToInt32(reader("Professor_ID")), reader("Password").ToString())
                End While
            End Using
        End Using

        roleComboBox.DataSource = choices
        roleComboBox.DisplayMember = "DisplayText"
        roleComboBox.ValueMember = "Professor_ID"

        If roleComboBox.Items.Count > 0 Then
            roleComboBox.SelectedIndex = 0
        End If

        passwordTextBox.Clear()
    End Sub

    Private Sub loginButton_Click(sender As Object, e As EventArgs) Handles loginButton.Click
        Dim selected = TryCast(roleComboBox.SelectedItem, DataRowView)
        If selected Is Nothing Then
            MessageBox.Show("Please select a login option.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim enteredPassword = passwordTextBox.Text.Trim()
        If String.IsNullOrWhiteSpace(enteredPassword) Then
            MessageBox.Show("Please enter a password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information)
            passwordTextBox.Focus()
            Return
        End If

        Dim storedPassword = selected("Password").ToString()
        If Not String.Equals(storedPassword, enteredPassword, StringComparison.Ordinal) Then
            MessageBox.Show("The password does not match the selected account.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            passwordTextBox.SelectAll()
            passwordTextBox.Focus()
            Return
        End If

        Dim isAdmin = Convert.ToBoolean(selected("IsAdmin"))
        Dim displayName = selected("DisplayText").ToString()

        Dim professorId As Integer? = Nothing
        If selected("Professor_ID") IsNot DBNull.Value Then
            professorId = Convert.ToInt32(selected("Professor_ID"))
        End If

        MessageBox.Show($"Welcome, {displayName}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
        RaiseEvent LoginSucceeded(Me, New RoleLoginEventArgs(isAdmin, professorId, displayName))
    End Sub
End Class