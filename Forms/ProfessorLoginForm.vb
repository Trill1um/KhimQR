Imports Microsoft.Data.Sqlite

Public Class ProfessorLoginForm
    Inherits Form

    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property SelectedProfessorId As Integer

    Private comboBox1 As New ComboBox()
    Private btnLogin As New Button()
    Private btnCancel As New Button()

    Public Sub New()
        Text = "Professor Login"
        Size = New Size(350, 180)
        StartPosition = FormStartPosition.CenterParent
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False

        Dim label1 As New Label() With {
            .Text = "Select your profile to start scanning:",
            .Location = New Point(20, 20),
            .AutoSize = True,
            .Font = New Font("Segoe UI", 10, FontStyle.Regular)
        }

        comboBox1.Location = New Point(20, 50)
        comboBox1.Width = 290
        comboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        comboBox1.Font = New Font("Segoe UI", 10, FontStyle.Regular)

        btnLogin.Text = "Login"
        btnLogin.DialogResult = DialogResult.OK
        btnLogin.Location = New Point(125, 95)
        btnLogin.Width = 90
        btnLogin.Height = 30

        btnCancel.Text = "Cancel"
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(220, 95)
        btnCancel.Width = 90
        btnCancel.Height = 30

        Controls.Add(label1)
        Controls.Add(comboBox1)
        Controls.Add(btnLogin)
        Controls.Add(btnCancel)

        AcceptButton = btnLogin
        CancelButton = btnCancel

        LoadProfessors()
    End Sub

    Private Sub LoadProfessors()
        If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
            connect()
        End If

        Dim dt As New DataTable()
        Const sql As String = "SELECT Professor_ID, FirstName || ' ' || CASE WHEN IFNULL(TRIM(MiddleName), '') = '' THEN '' ELSE SUBSTR(TRIM(MiddleName), 1, 1) || '. ' END || LastName AS FullName FROM Professor ORDER BY LastName, FirstName"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                dt.Load(reader)
            End Using
        End Using

        comboBox1.DataSource = dt
        comboBox1.DisplayMember = "FullName"
        comboBox1.ValueMember = "Professor_ID"
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        If DialogResult = DialogResult.OK Then
            If comboBox1.SelectedValue IsNot Nothing Then
                SelectedProfessorId = Convert.ToInt32(comboBox1.SelectedValue)
            Else
                MessageBox.Show("Please select a professor.")
                e.Cancel = True
            End If
        End If
        MyBase.OnFormClosing(e)
    End Sub
End Class
