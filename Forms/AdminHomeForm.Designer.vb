<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdminHomeForm
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
        adminButton = New Button()
        addButton = New Button()
        attendanceButton = New Button()
        enrollmentButton = New Button()
        SuspendLayout()
        ' 
        ' titleLabel
        ' 
        titleLabel.AutoSize = True
        titleLabel.Font = New Font("Segoe UI", 36.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        titleLabel.Location = New Point(323, 35)
        titleLabel.Name = "titleLabel"
        titleLabel.Size = New Size(387, 81)
        titleLabel.TabIndex = 0
        titleLabel.Text = "Admin Home"
        titleLabel.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' adminButton
        ' 
        adminButton.Font = New Font("Segoe UI", 12.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        adminButton.Location = New Point(151, 179)
        adminButton.Name = "adminButton"
        adminButton.Size = New Size(277, 68)
        adminButton.TabIndex = 1
        adminButton.Text = "Show Admin Panel"
        adminButton.UseVisualStyleBackColor = True
        ' 
        ' addButton
        ' 
        addButton.Font = New Font("Segoe UI", 12.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        addButton.Location = New Point(563, 179)
        addButton.Name = "addButton"
        addButton.Size = New Size(277, 68)
        addButton.TabIndex = 2
        addButton.Text = "Add Data"
        addButton.UseVisualStyleBackColor = True
        ' 
        ' attendanceButton
        ' 
        attendanceButton.Font = New Font("Segoe UI", 12.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        attendanceButton.Location = New Point(563, 275)
        attendanceButton.Name = "attendanceButton"
        attendanceButton.Size = New Size(277, 68)
        attendanceButton.TabIndex = 3
        attendanceButton.Text = "Check Attendance"
        attendanceButton.UseVisualStyleBackColor = True
        ' 
        ' enrollmentButton
        ' 
        enrollmentButton.Font = New Font("Segoe UI", 12.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        enrollmentButton.Location = New Point(151, 275)
        enrollmentButton.Name = "enrollmentButton"
        enrollmentButton.Size = New Size(277, 68)
        enrollmentButton.TabIndex = 4
        enrollmentButton.Text = "Enrollment"
        enrollmentButton.UseVisualStyleBackColor = True
        ' 
        ' AdminHomeForm
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1004, 450)
        Controls.Add(enrollmentButton)
        Controls.Add(attendanceButton)
        Controls.Add(addButton)
        Controls.Add(adminButton)
        Controls.Add(titleLabel)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Name = "AdminHomeForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Admin Home"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents titleLabel As Label
    Friend WithEvents adminButton As Button
    Friend WithEvents addButton As Button
    Friend WithEvents attendanceButton As Button
    Friend WithEvents enrollmentButton As Button
End Class