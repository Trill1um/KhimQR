<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProfessorAttendancePanelForm
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
        topPanel = New Panel()
        clearFiltersButton = New Button()
        sessionLabel = New Label()
        sessionComboBox = New ComboBox()
        courseLabel = New Label()
        courseComboBox = New ComboBox()
        dateLabel = New Label()
        attendanceDatePicker = New DateTimePicker()
        attendanceGrid = New DataGridView()
        topPanel.SuspendLayout()
        CType(attendanceGrid, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' topPanel
        ' 
        topPanel.Controls.Add(clearFiltersButton)
        topPanel.Controls.Add(sessionLabel)
        topPanel.Controls.Add(sessionComboBox)
        topPanel.Controls.Add(courseLabel)
        topPanel.Controls.Add(courseComboBox)
        topPanel.Controls.Add(dateLabel)
        topPanel.Controls.Add(attendanceDatePicker)
        topPanel.Dock = DockStyle.Top
        topPanel.Location = New Point(0, 0)
        topPanel.Name = "topPanel"
        topPanel.Size = New Size(1004, 56)
        topPanel.TabIndex = 0
        ' 
        ' clearFiltersButton
        ' 
        clearFiltersButton.Location = New Point(862, 12)
        clearFiltersButton.Name = "clearFiltersButton"
        clearFiltersButton.Size = New Size(130, 30)
        clearFiltersButton.TabIndex = 6
        clearFiltersButton.Text = "Clear Filters"
        clearFiltersButton.UseVisualStyleBackColor = True
        ' 
        ' sessionLabel
        ' 
        sessionLabel.AutoSize = True
        sessionLabel.Location = New Point(568, 17)
        sessionLabel.Name = "sessionLabel"
        sessionLabel.Size = New Size(56, 20)
        sessionLabel.TabIndex = 5
        sessionLabel.Text = "Session"
        ' 
        ' sessionComboBox
        ' 
        sessionComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        sessionComboBox.FormattingEnabled = True
        sessionComboBox.Location = New Point(630, 13)
        sessionComboBox.Name = "sessionComboBox"
        sessionComboBox.Size = New Size(220, 28)
        sessionComboBox.TabIndex = 4
        ' 
        ' courseLabel
        ' 
        courseLabel.AutoSize = True
        courseLabel.Location = New Point(284, 17)
        courseLabel.Name = "courseLabel"
        courseLabel.Size = New Size(54, 20)
        courseLabel.TabIndex = 3
        courseLabel.Text = "Course"
        ' 
        ' courseComboBox
        ' 
        courseComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        courseComboBox.FormattingEnabled = True
        courseComboBox.Location = New Point(344, 13)
        courseComboBox.Name = "courseComboBox"
        courseComboBox.Size = New Size(210, 28)
        courseComboBox.TabIndex = 2
        ' 
        ' dateLabel
        ' 
        dateLabel.AutoSize = True
        dateLabel.Location = New Point(12, 17)
        dateLabel.Name = "dateLabel"
        dateLabel.Size = New Size(41, 20)
        dateLabel.TabIndex = 1
        dateLabel.Text = "Date"
        ' 
        ' attendanceDatePicker
        ' 
        attendanceDatePicker.CustomFormat = "MMMM dd, yyyy"
        attendanceDatePicker.Format = DateTimePickerFormat.Custom
        attendanceDatePicker.Location = New Point(59, 13)
        attendanceDatePicker.Name = "attendanceDatePicker"
        attendanceDatePicker.Size = New Size(210, 27)
        attendanceDatePicker.TabIndex = 0
        ' 
        ' attendanceGrid
        ' 
        attendanceGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        attendanceGrid.Dock = DockStyle.Fill
        attendanceGrid.Location = New Point(0, 56)
        attendanceGrid.Name = "attendanceGrid"
        attendanceGrid.RowHeadersWidth = 51
        attendanceGrid.Size = New Size(1004, 394)
        attendanceGrid.TabIndex = 1
        ' 
        ' ProfessorAttendancePanelForm
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1004, 450)
        Controls.Add(attendanceGrid)
        Controls.Add(topPanel)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Name = "ProfessorAttendancePanelForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Professor Attendance Panel"
        topPanel.ResumeLayout(False)
        topPanel.PerformLayout()
        CType(attendanceGrid, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents topPanel As Panel
    Friend WithEvents dateLabel As Label
    Friend WithEvents attendanceDatePicker As DateTimePicker
    Friend WithEvents courseLabel As Label
    Friend WithEvents courseComboBox As ComboBox
    Friend WithEvents sessionLabel As Label
    Friend WithEvents sessionComboBox As ComboBox
    Friend WithEvents clearFiltersButton As Button
    Friend WithEvents attendanceGrid As DataGridView
End Class
