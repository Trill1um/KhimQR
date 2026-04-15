<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProfessorHomeForm
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
        titleLabel = New Label()
        professorPanelButton = New Button()
        attendanceButton = New Button()
        SuspendLayout()
        ' 
        ' titleLabel
        ' 
        titleLabel.AutoSize = True
        titleLabel.Font = New Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        titleLabel.Location = New Point(204, 38)
        titleLabel.Name = "titleLabel"
        titleLabel.Size = New Size(603, 81)
        titleLabel.TabIndex = 0
        titleLabel.Text = "Professor Home Page"
        titleLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' professorPanelButton
        ' 
        professorPanelButton.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        professorPanelButton.Location = New Point(208, 204)
        professorPanelButton.Name = "professorPanelButton"
        professorPanelButton.Size = New Size(253, 76)
        professorPanelButton.TabIndex = 1
        professorPanelButton.Text = "Professor Panel"
        professorPanelButton.UseVisualStyleBackColor = True
        ' 
        ' attendanceButton
        ' 
        attendanceButton.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        attendanceButton.Location = New Point(548, 204)
        attendanceButton.Name = "attendanceButton"
        attendanceButton.Size = New Size(253, 76)
        attendanceButton.TabIndex = 2
        attendanceButton.Text = "Check Attendance"
        attendanceButton.UseVisualStyleBackColor = True
        ' 
        ' ProfessorHomeForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1004, 450)
        Controls.Add(attendanceButton)
        Controls.Add(professorPanelButton)
        Controls.Add(titleLabel)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Name = "ProfessorHomeForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Professor Home"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents titleLabel As Label
    Friend WithEvents professorPanelButton As Button
    Friend WithEvents attendanceButton As Button
End Class