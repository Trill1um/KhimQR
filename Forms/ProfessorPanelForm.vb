Imports Microsoft.Data.Sqlite

Public Class ProfessorPanelForm
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
    Public Property CurrentProfessorId As Integer

    Public Sub New()
        InitializeComponent()
        ConfigureGrid()
    End Sub

    Private Sub ConfigureGrid()
        studentsGrid.ReadOnly = True
        studentsGrid.AllowUserToAddRows = False
        studentsGrid.AllowUserToDeleteRows = False
        studentsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        studentsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        studentsGrid.MultiSelect = False
    End Sub

    Private Sub ProfessorPanelForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            LoadCourses()
            LoadSections()
            LoadStudents()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Professor Panel", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub LoadCourses()
        EnsureConnection()

        Dim rawTable As New DataTable()
        Const sql As String = "SELECT DISTINCT c.Course_ID, c.Code || ' - ' || c.Name AS DisplayText " &
                            "FROM ClassSection cs " &
                            "JOIN Course c ON cs.Course_ID = c.Course_ID " &
                            "WHERE (@profId = 0 OR cs.Professor_ID = @profId) " &
                            "ORDER BY c.Code"

        Using cmd As New SqliteCommand(sql, sqlconn)
            cmd.Parameters.AddWithValue("@profId", CurrentProfessorId)
            Using reader = cmd.ExecuteReader()
                rawTable.Load(reader)
            End Using
        End Using

        Dim table As New DataTable()
        table.Columns.Add("Course_ID", GetType(Integer))
        table.Columns.Add("DisplayText", GetType(String))
        table.Rows.Add(DBNull.Value, String.Empty)

        For Each row As DataRow In rawTable.Rows
            table.Rows.Add(Convert.ToInt32(row("Course_ID")), Convert.ToString(row("DisplayText")))
        Next

        courseComboBox.DataSource = table
        courseComboBox.DisplayMember = "DisplayText"
        courseComboBox.ValueMember = "Course_ID"
        courseComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadSections(Optional courseId As Integer? = Nothing)
        EnsureConnection()

        Dim rawTable As New DataTable()
        Dim sql As String = "SELECT DISTINCT cs.ClassSection_ID, c.Code || ' - ' || cs.SectionName AS DisplayText " &
                            "FROM ClassSection cs " &
                            "JOIN Course c ON cs.Course_ID = c.Course_ID " &
                            "WHERE (@profId = 0 OR cs.Professor_ID = @profId)"

        If courseId.HasValue Then
            sql &= " AND cs.Course_ID = @courseId"
        End If

        sql &= " ORDER BY c.Code, cs.SectionName"

        Using cmd As New SqliteCommand(sql, sqlconn)
            cmd.Parameters.AddWithValue("@profId", CurrentProfessorId)
            If courseId.HasValue Then
                cmd.Parameters.AddWithValue("@courseId", courseId.Value)
            End If

            Using reader = cmd.ExecuteReader()
                rawTable.Load(reader)
            End Using
        End Using

        Dim table As New DataTable()
        table.Columns.Add("ClassSection_ID", GetType(Integer))
        table.Columns.Add("DisplayText", GetType(String))
        table.Rows.Add(DBNull.Value, String.Empty)

        For Each row As DataRow In rawTable.Rows
            table.Rows.Add(Convert.ToInt32(row("ClassSection_ID")), Convert.ToString(row("DisplayText")))
        Next

        sectionComboBox.DataSource = table
        sectionComboBox.DisplayMember = "DisplayText"
        sectionComboBox.ValueMember = "ClassSection_ID"
        sectionComboBox.SelectedIndex = 0
    End Sub

    Private Sub LoadStudents(Optional courseId As Integer? = Nothing, Optional classSectionId As Integer? = Nothing)
        EnsureConnection()

        Dim table As New DataTable()
        Const sql As String = "SELECT s.Student_Code AS StudentCode, " &
                            "(s.LastName || ', ' || s.FirstName || CASE WHEN IFNULL(s.MiddleName, '') = '' THEN '' ELSE ' ' || s.MiddleName END) AS StudentName, " &
                            "c.Code AS CourseCode, c.Name AS CourseName, cs.SectionName " &
                            "FROM Enrollment e " &
                            "JOIN Student s ON e.Student_ID = s.ID " &
                            "JOIN ClassSection cs ON e.ClassSection_ID = cs.ClassSection_ID " &
                            "JOIN Course c ON cs.Course_ID = c.Course_ID " &
                            "WHERE (@profId = 0 OR cs.Professor_ID = @profId) " &
                            "AND (@courseId IS NULL OR cs.Course_ID = @courseId) " &
                            "AND (@classSectionId IS NULL OR cs.ClassSection_ID = @classSectionId) " &
                            "ORDER BY c.Code, cs.SectionName, s.LastName, s.FirstName"

        Using cmd As New SqliteCommand(sql, sqlconn)
            cmd.Parameters.AddWithValue("@profId", CurrentProfessorId)

            Dim courseParam As New SqliteParameter("@courseId", DbType.Int32)
            courseParam.Value = If(courseId.HasValue, courseId.Value, CType(DBNull.Value, Object))
            cmd.Parameters.Add(courseParam)

            Dim sectionParam As New SqliteParameter("@classSectionId", DbType.Int32)
            sectionParam.Value = If(classSectionId.HasValue, classSectionId.Value, CType(DBNull.Value, Object))
            cmd.Parameters.Add(sectionParam)

            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using

        studentsGrid.DataSource = table
    End Sub

    Private Function GetSelectedInt(combo As ComboBox) As Integer?
        If combo.SelectedValue Is Nothing OrElse combo.SelectedValue Is DBNull.Value Then
            Return Nothing
        End If

        Dim value As Integer
        If Integer.TryParse(combo.SelectedValue.ToString(), value) Then
            Return value
        End If

        Return Nothing
    End Function

    Private Sub ApplyFilters()
        LoadStudents(GetSelectedInt(courseComboBox), GetSelectedInt(sectionComboBox))
    End Sub

    Private Sub courseComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles courseComboBox.SelectedIndexChanged
        LoadSections(GetSelectedInt(courseComboBox))
        ApplyFilters()
    End Sub

    Private Sub sectionComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sectionComboBox.SelectedIndexChanged
        ApplyFilters()
    End Sub

    Private Sub clearFiltersButton_Click(sender As Object, e As EventArgs) Handles clearFiltersButton.Click
        courseComboBox.SelectedIndex = 0
        LoadSections()
        sectionComboBox.SelectedIndex = 0
        LoadStudents()
    End Sub

    Private Sub EnsureConnection()
        If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
            connect()
        End If
    End Sub
End Class