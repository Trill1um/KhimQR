<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HomeForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Label1 = New Label()
        Button1 = New Button()
        Button2 = New Button()
        Button3 = New Button()
        Button4 = New Button()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(212, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(574, 162)
        Label1.TabIndex = 1
        Label1.Text = "Student Attendance " & vbCrLf & "Tracker"
        Label1.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Button1
        ' 
        Button1.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Button1.Location = New Point(54, 230)
        Button1.Name = "Button1"
        Button1.Size = New Size(221, 68)
        Button1.TabIndex = 2
        Button1.TabStop = False
        Button1.Text = "Show Admin Panel"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Button2.Location = New Point(375, 230)
        Button2.Name = "Button2"
        Button2.Size = New Size(221, 68)
        Button2.TabIndex = 3
        Button2.TabStop = False
        Button2.Text = "Add Data"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Button3.Location = New Point(728, 230)
        Button3.Name = "Button3"
        Button3.Size = New Size(221, 68)
        Button3.TabIndex = 4
        Button3.TabStop = False
        Button3.Text = "Check Attendance"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Button4.Location = New Point(375, 314)
        Button4.Name = "Button4"
        Button4.Size = New Size(221, 68)
        Button4.TabIndex = 5
        Button4.TabStop = False
        Button4.Text = "Enrollment"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' HomeForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1004, 450)
        Controls.Add(Button4)
        Controls.Add(Button3)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Controls.Add(Label1)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Name = "HomeForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Home"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
End Class
