Imports Microsoft.Data.Sqlite

Public Class AdminForm
    Inherits Form

    Private ReadOnly tabControl As New TabControl()
    Private ReadOnly studentsPage As New TabPage("Students")
    Private ReadOnly autonumberPage As New TabPage("Autonumber")
    Private ReadOnly studentsGrid As New DataGridView()
    Private ReadOnly autonumberGrid As New DataGridView()
    Private ReadOnly refreshButton As New Button()

    Public Sub New()
        Text = "Admin - Database Viewer"
        Width = 950
        Height = 550
        StartPosition = FormStartPosition.CenterParent

        refreshButton.Text = "Refresh"
        refreshButton.Width = 120
        refreshButton.Height = 32
        refreshButton.Top = 10
        refreshButton.Left = 10

        tabControl.Left = 10
        tabControl.Top = refreshButton.Bottom + 10
        tabControl.Width = ClientSize.Width - 20
        tabControl.Height = ClientSize.Height - tabControl.Top - 10
        tabControl.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right

        ConfigureGrid(studentsGrid)
        ConfigureGrid(autonumberGrid)

        studentsPage.Controls.Add(studentsGrid)
        autonumberPage.Controls.Add(autonumberGrid)

        tabControl.TabPages.Add(studentsPage)
        tabControl.TabPages.Add(autonumberPage)

        Controls.Add(refreshButton)
        Controls.Add(tabControl)

        AddHandler refreshButton.Click, AddressOf RefreshButton_Click
        AddHandler Load, AddressOf AdminForm_Load
    End Sub

    Private Sub ConfigureGrid(grid As DataGridView)
        grid.Dock = DockStyle.Fill
        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.MultiSelect = False
    End Sub

    Private Sub AdminForm_Load(sender As Object, e As EventArgs)
        LoadData()
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs)
        LoadData()
    End Sub

    Private Sub LoadData()
        Try
            If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
                connect()
            End If

            LoadStudents()
            LoadAutonumber()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Admin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub LoadStudents()
        Dim table As New DataTable()
        Const sql As String = "SELECT StudentID, Firstname, Middlename, Lastname, Course, Section, length(QRCode) AS QRBytes FROM StudentMasterLists ORDER BY StudentID"

        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using

        studentsGrid.DataSource = table
    End Sub

    Private Sub LoadAutonumber()
        Dim table As New DataTable()
        Const sql As String = "SELECT pfx, NewNumber FROM Autonumber ORDER BY pfx"

        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using

        autonumberGrid.DataSource = table
    End Sub
End Class
