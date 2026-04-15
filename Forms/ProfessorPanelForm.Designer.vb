<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProfessorPanelForm
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
        sectionLabel = New Label()
        sectionComboBox = New ComboBox()
        courseLabel = New Label()
        courseComboBox = New ComboBox()
        studentsGrid = New DataGridView()
        topPanel.SuspendLayout()
        CType(studentsGrid, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' topPanel
        ' 
        topPanel.Controls.Add(clearFiltersButton)
        topPanel.Controls.Add(sectionLabel)
        topPanel.Controls.Add(sectionComboBox)
        topPanel.Controls.Add(courseLabel)
        topPanel.Controls.Add(courseComboBox)
        topPanel.Dock = DockStyle.Top
        topPanel.Location = New Point(0, 0)
        topPanel.Name = "topPanel"
        topPanel.Size = New Size(1004, 56)
        topPanel.TabIndex = 0
        ' 
        ' clearFiltersButton
        ' 
        clearFiltersButton.Location = New Point(862, 11)
        clearFiltersButton.Name = "clearFiltersButton"
        clearFiltersButton.Size = New Size(130, 30)
        clearFiltersButton.TabIndex = 4
        clearFiltersButton.Text = "Clear Filters"
        clearFiltersButton.UseVisualStyleBackColor = True
        ' 
        ' sectionLabel
        ' 
        sectionLabel.AutoSize = True
        sectionLabel.Location = New Point(366, 16)
        sectionLabel.Name = "sectionLabel"
        sectionLabel.Size = New Size(58, 20)
        sectionLabel.TabIndex = 3
        sectionLabel.Text = "Section"
        ' 
        ' sectionComboBox
        ' 
        sectionComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        sectionComboBox.FormattingEnabled = True
        sectionComboBox.Location = New Point(426, 12)
        sectionComboBox.Name = "sectionComboBox"
        sectionComboBox.Size = New Size(280, 28)
        sectionComboBox.TabIndex = 2
        ' 
        ' courseLabel
        ' 
        courseLabel.AutoSize = True
        courseLabel.Location = New Point(12, 16)
        courseLabel.Name = "courseLabel"
        courseLabel.Size = New Size(54, 20)
        courseLabel.TabIndex = 1
        courseLabel.Text = "Course"
        ' 
        ' courseComboBox
        ' 
        courseComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        courseComboBox.FormattingEnabled = True
        courseComboBox.Location = New Point(72, 12)
        courseComboBox.Name = "courseComboBox"
        courseComboBox.Size = New Size(280, 28)
        courseComboBox.TabIndex = 0
        ' 
        ' studentsGrid
        ' 
        studentsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        studentsGrid.Dock = DockStyle.Fill
        studentsGrid.Location = New Point(0, 56)
        studentsGrid.Name = "studentsGrid"
        studentsGrid.RowHeadersWidth = 51
        studentsGrid.Size = New Size(1004, 394)
        studentsGrid.TabIndex = 1
        ' 
        ' ProfessorPanelForm
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1004, 450)
        Controls.Add(studentsGrid)
        Controls.Add(topPanel)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Name = "ProfessorPanelForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Professor Panel"
        topPanel.ResumeLayout(False)
        topPanel.PerformLayout()
        CType(studentsGrid, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents topPanel As Panel
    Friend WithEvents courseComboBox As ComboBox
    Friend WithEvents courseLabel As Label
    Friend WithEvents sectionComboBox As ComboBox
    Friend WithEvents sectionLabel As Label
    Friend WithEvents clearFiltersButton As Button
    Friend WithEvents studentsGrid As DataGridView
End Class