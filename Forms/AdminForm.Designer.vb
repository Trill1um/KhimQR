<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdminForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        refreshButton = New Button()
        resetButton = New Button()
        tabControl = New TabControl()
        profPage = New TabPage()
        profGrid = New DataGridView()
        coursePage = New TabPage()
        courseGrid = New DataGridView()
        studentPage = New TabPage()
        studentGrid = New DataGridView()
        classSectionPage = New TabPage()
        classSectionGrid = New DataGridView()
        classSessionPage = New TabPage()
        classSessionGrid = New DataGridView()
        enrollmentPage = New TabPage()
        enrollmentGrid = New DataGridView()
        attendancePage = New TabPage()
        attendanceGrid = New DataGridView()
        tabControl.SuspendLayout()
        profPage.SuspendLayout()
        CType(profGrid, ComponentModel.ISupportInitialize).BeginInit()
        coursePage.SuspendLayout()
        CType(courseGrid, ComponentModel.ISupportInitialize).BeginInit()
        studentPage.SuspendLayout()
        CType(studentGrid, ComponentModel.ISupportInitialize).BeginInit()
        classSectionPage.SuspendLayout()
        CType(classSectionGrid, ComponentModel.ISupportInitialize).BeginInit()
        classSessionPage.SuspendLayout()
        CType(classSessionGrid, ComponentModel.ISupportInitialize).BeginInit()
        enrollmentPage.SuspendLayout()
        CType(enrollmentGrid, ComponentModel.ISupportInitialize).BeginInit()
        attendancePage.SuspendLayout()
        CType(attendanceGrid, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' refreshButton
        ' 
        refreshButton.Location = New Point(12, 12)
        refreshButton.Name = "refreshButton"
        refreshButton.Size = New Size(120, 32)
        refreshButton.TabIndex = 0
        refreshButton.Text = "Refresh"
        refreshButton.UseVisualStyleBackColor = True
        ' 
        ' resetButton
        ' 
        resetButton.Location = New Point(138, 12)
        resetButton.Name = "resetButton"
        resetButton.Size = New Size(120, 32)
        resetButton.TabIndex = 1
        resetButton.Text = "Reset DB"
        resetButton.UseVisualStyleBackColor = True
        ' 
        ' tabControl
        ' 
        tabControl.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        tabControl.Controls.Add(profPage)
        tabControl.Controls.Add(coursePage)
        tabControl.Controls.Add(studentPage)
        tabControl.Controls.Add(classSectionPage)
        tabControl.Controls.Add(classSessionPage)
        tabControl.Controls.Add(enrollmentPage)
        tabControl.Controls.Add(attendancePage)
        tabControl.Location = New Point(12, 50)
        tabControl.Name = "tabControl"
        tabControl.SelectedIndex = 0
        tabControl.Size = New Size(980, 388)
        tabControl.TabIndex = 2
        ' 
        ' profPage
        ' 
        profPage.Controls.Add(profGrid)
        profPage.Location = New Point(4, 29)
        profPage.Name = "profPage"
        profPage.Padding = New Padding(3)
        profPage.Size = New Size(972, 355)
        profPage.TabIndex = 0
        profPage.Text = "Professor"
        profPage.UseVisualStyleBackColor = True
        ' 
        ' profGrid
        ' 
        profGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        profGrid.Dock = DockStyle.Fill
        profGrid.Location = New Point(3, 3)
        profGrid.Name = "profGrid"
        profGrid.RowHeadersWidth = 51
        profGrid.Size = New Size(966, 349)
        profGrid.TabIndex = 0
        ' 
        ' coursePage
        ' 
        coursePage.Controls.Add(courseGrid)
        coursePage.Location = New Point(4, 29)
        coursePage.Name = "coursePage"
        coursePage.Padding = New Padding(3)
        coursePage.Size = New Size(918, 455)
        coursePage.TabIndex = 1
        coursePage.Text = "Course"
        coursePage.UseVisualStyleBackColor = True
        ' 
        ' courseGrid
        ' 
        courseGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        courseGrid.Dock = DockStyle.Fill
        courseGrid.Location = New Point(3, 3)
        courseGrid.Name = "courseGrid"
        courseGrid.RowHeadersWidth = 51
        courseGrid.Size = New Size(912, 449)
        courseGrid.TabIndex = 0
        ' 
        ' studentPage
        ' 
        studentPage.Controls.Add(studentGrid)
        studentPage.Location = New Point(4, 29)
        studentPage.Name = "studentPage"
        studentPage.Padding = New Padding(3)
        studentPage.Size = New Size(918, 455)
        studentPage.TabIndex = 2
        studentPage.Text = "Student"
        studentPage.UseVisualStyleBackColor = True
        ' 
        ' studentGrid
        ' 
        studentGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        studentGrid.Dock = DockStyle.Fill
        studentGrid.Location = New Point(3, 3)
        studentGrid.Name = "studentGrid"
        studentGrid.RowHeadersWidth = 51
        studentGrid.Size = New Size(912, 449)
        studentGrid.TabIndex = 0
        ' 
        ' classSectionPage
        ' 
        classSectionPage.Controls.Add(classSectionGrid)
        classSectionPage.Location = New Point(4, 29)
        classSectionPage.Name = "classSectionPage"
        classSectionPage.Padding = New Padding(3)
        classSectionPage.Size = New Size(918, 455)
        classSectionPage.TabIndex = 3
        classSectionPage.Text = "ClassSection"
        classSectionPage.UseVisualStyleBackColor = True
        ' 
        ' classSectionGrid
        ' 
        classSectionGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        classSectionGrid.Dock = DockStyle.Fill
        classSectionGrid.Location = New Point(3, 3)
        classSectionGrid.Name = "classSectionGrid"
        classSectionGrid.RowHeadersWidth = 51
        classSectionGrid.Size = New Size(912, 449)
        classSectionGrid.TabIndex = 0
        ' 
        ' classSessionPage
        ' 
        classSessionPage.Controls.Add(classSessionGrid)
        classSessionPage.Location = New Point(4, 29)
        classSessionPage.Name = "classSessionPage"
        classSessionPage.Padding = New Padding(3)
        classSessionPage.Size = New Size(918, 455)
        classSessionPage.TabIndex = 4
        classSessionPage.Text = "ClassSession"
        classSessionPage.UseVisualStyleBackColor = True
        ' 
        ' classSessionGrid
        ' 
        classSessionGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        classSessionGrid.Dock = DockStyle.Fill
        classSessionGrid.Location = New Point(3, 3)
        classSessionGrid.Name = "classSessionGrid"
        classSessionGrid.RowHeadersWidth = 51
        classSessionGrid.Size = New Size(912, 449)
        classSessionGrid.TabIndex = 0
        ' 
        ' enrollmentPage
        ' 
        enrollmentPage.Controls.Add(enrollmentGrid)
        enrollmentPage.Location = New Point(4, 29)
        enrollmentPage.Name = "enrollmentPage"
        enrollmentPage.Padding = New Padding(3)
        enrollmentPage.Size = New Size(918, 455)
        enrollmentPage.TabIndex = 5
        enrollmentPage.Text = "Enrollment"
        enrollmentPage.UseVisualStyleBackColor = True
        ' 
        ' enrollmentGrid
        ' 
        enrollmentGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        enrollmentGrid.Dock = DockStyle.Fill
        enrollmentGrid.Location = New Point(3, 3)
        enrollmentGrid.Name = "enrollmentGrid"
        enrollmentGrid.RowHeadersWidth = 51
        enrollmentGrid.Size = New Size(912, 449)
        enrollmentGrid.TabIndex = 0
        ' 
        ' attendancePage
        ' 
        attendancePage.Controls.Add(attendanceGrid)
        attendancePage.Location = New Point(4, 29)
        attendancePage.Name = "attendancePage"
        attendancePage.Padding = New Padding(3)
        attendancePage.Size = New Size(918, 455)
        attendancePage.TabIndex = 6
        attendancePage.Text = "Attendance"
        attendancePage.UseVisualStyleBackColor = True
        ' 
        ' attendanceGrid
        ' 
        attendanceGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        attendanceGrid.Dock = DockStyle.Fill
        attendanceGrid.Location = New Point(3, 3)
        attendanceGrid.Name = "attendanceGrid"
        attendanceGrid.RowHeadersWidth = 51
        attendanceGrid.Size = New Size(912, 449)
        attendanceGrid.TabIndex = 0
        ' 
        ' AdminForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1004, 450)
        Controls.Add(tabControl)
        Controls.Add(resetButton)
        Controls.Add(refreshButton)
        Name = "AdminForm"
        StartPosition = FormStartPosition.CenterParent
        Text = "Admin - Database Viewer"
        tabControl.ResumeLayout(False)
        profPage.ResumeLayout(False)
        CType(profGrid, ComponentModel.ISupportInitialize).EndInit()
        coursePage.ResumeLayout(False)
        CType(courseGrid, ComponentModel.ISupportInitialize).EndInit()
        studentPage.ResumeLayout(False)
        CType(studentGrid, ComponentModel.ISupportInitialize).EndInit()
        classSectionPage.ResumeLayout(False)
        CType(classSectionGrid, ComponentModel.ISupportInitialize).EndInit()
        classSessionPage.ResumeLayout(False)
        CType(classSessionGrid, ComponentModel.ISupportInitialize).EndInit()
        enrollmentPage.ResumeLayout(False)
        CType(enrollmentGrid, ComponentModel.ISupportInitialize).EndInit()
        attendancePage.ResumeLayout(False)
        CType(attendanceGrid, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents refreshButton As Button
    Friend WithEvents resetButton As Button
    Friend WithEvents tabControl As TabControl
    Friend WithEvents profPage As TabPage
    Friend WithEvents coursePage As TabPage
    Friend WithEvents studentPage As TabPage
    Friend WithEvents classSectionPage As TabPage
    Friend WithEvents classSessionPage As TabPage
    Friend WithEvents enrollmentPage As TabPage
    Friend WithEvents attendancePage As TabPage
    Friend WithEvents profGrid As DataGridView
    Friend WithEvents courseGrid As DataGridView
    Friend WithEvents studentGrid As DataGridView
    Friend WithEvents classSectionGrid As DataGridView
    Friend WithEvents classSessionGrid As DataGridView
    Friend WithEvents enrollmentGrid As DataGridView
    Friend WithEvents attendanceGrid As DataGridView
End Class