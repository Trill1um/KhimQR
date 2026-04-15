<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EnrollmentForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        TextBox1 = New TextBox()
        TextBox2 = New TextBox()
        TextBox3 = New TextBox()
        TextBox4 = New TextBox()
        ComboBox1 = New ComboBox()
        ComboBox2 = New ComboBox()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        Button3 = New Button()
        SuspendLayout()
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(338, 57)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(415, 27)
        TextBox1.TabIndex = 0
        ' 
        ' TextBox2
        ' 
        TextBox2.Enabled = False
        TextBox2.Location = New Point(338, 100)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(415, 27)
        TextBox2.TabIndex = 1
        ' 
        ' TextBox3
        ' 
        TextBox3.Enabled = False
        TextBox3.Location = New Point(338, 186)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(415, 27)
        TextBox3.TabIndex = 3
        ' 
        ' TextBox4
        ' 
        TextBox4.Enabled = False
        TextBox4.Location = New Point(338, 143)
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(415, 27)
        TextBox4.TabIndex = 2
        ' 
        ' ComboBox1
        ' 
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox1.FormattingEnabled = True
        ComboBox1.Location = New Point(338, 229)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(415, 28)
        ComboBox1.TabIndex = 4
        ' 
        ' ComboBox2
        ' 
        ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox2.FormattingEnabled = True
        ComboBox2.Location = New Point(338, 272)
        ComboBox2.Name = "ComboBox2"
        ComboBox2.Size = New Size(415, 28)
        ComboBox2.TabIndex = 5
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(183, 60)
        Label1.Name = "Label1"
        Label1.Size = New Size(79, 20)
        Label1.TabIndex = 6
        Label1.Text = "Student ID"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(183, 103)
        Label2.Name = "Label2"
        Label2.Size = New Size(76, 20)
        Label2.TabIndex = 7
        Label2.Text = "FirstName"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(183, 146)
        Label3.Name = "Label3"
        Label3.Size = New Size(96, 20)
        Label3.TabIndex = 6
        Label3.Text = "MiddleName"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(183, 189)
        Label4.Name = "Label4"
        Label4.Size = New Size(75, 20)
        Label4.TabIndex = 7
        Label4.Text = "LastName"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(183, 232)
        Label5.Name = "Label5"
        Label5.Size = New Size(54, 20)
        Label5.TabIndex = 6
        Label5.Text = "Course"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(183, 275)
        Label6.Name = "Label6"
        Label6.Size = New Size(58, 20)
        Label6.TabIndex = 7
        Label6.Text = "Section"
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(570, 322)
        Button3.Name = "Button3"
        Button3.Size = New Size(183, 29)
        Button3.TabIndex = 6
        Button3.Text = "ENROLL STUDENT"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' EnrollmentForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1004, 450)
        Controls.Add(Button3)
        Controls.Add(Label6)
        Controls.Add(Label4)
        Controls.Add(Label2)
        Controls.Add(Label5)
        Controls.Add(Label3)
        Controls.Add(Label1)
        Controls.Add(ComboBox2)
        Controls.Add(ComboBox1)
        Controls.Add(TextBox3)
        Controls.Add(TextBox4)
        Controls.Add(TextBox2)
        Controls.Add(TextBox1)
        Name = "EnrollmentForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Enrollment"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Button3 As Button
End Class
