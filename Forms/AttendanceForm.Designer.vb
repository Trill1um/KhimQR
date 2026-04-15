<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AttendanceForm
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
        PictureBox1 = New PictureBox()
        Button2 = New Button()
        TextBox1 = New TextBox()
        TextBox2 = New TextBox()
        TextBox4 = New TextBox()
        TextBox5 = New TextBox()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label5 = New Label()
        TextBox3 = New TextBox()
        Label6 = New Label()
        TextBox6 = New TextBox()
        Label4 = New Label()
        Label7 = New Label()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' PictureBox1
        ' 
        PictureBox1.BackColor = SystemColors.Control
        PictureBox1.Location = New Point(12, 12)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(493, 426)
        PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
        PictureBox1.TabIndex = 0
        PictureBox1.TabStop = False
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(12, 518)
        Button2.Name = "Button2"
        Button2.Size = New Size(94, 29)
        Button2.TabIndex = 1
        Button2.Text = "Stop"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' TextBox1
        ' 
        TextBox1.BackColor = SystemColors.HighlightText
        TextBox1.Enabled = False
        TextBox1.Location = New Point(633, 12)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(340, 27)
        TextBox1.TabIndex = 3
        ' 
        ' TextBox2
        ' 
        TextBox2.BackColor = SystemColors.HighlightText
        TextBox2.Enabled = False
        TextBox2.Location = New Point(633, 51)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(340, 27)
        TextBox2.TabIndex = 3
        ' 
        ' TextBox4
        ' 
        TextBox4.BackColor = SystemColors.HighlightText
        TextBox4.Enabled = False
        TextBox4.Location = New Point(633, 93)
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(128, 27)
        TextBox4.TabIndex = 3
        ' 
        ' TextBox5
        ' 
        TextBox5.BackColor = SystemColors.HighlightText
        TextBox5.Enabled = False
        TextBox5.Location = New Point(633, 132)
        TextBox5.Name = "TextBox5"
        TextBox5.Size = New Size(340, 27)
        TextBox5.TabIndex = 3
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(536, 15)
        Label1.Name = "Label1"
        Label1.Size = New Size(75, 20)
        Label1.TabIndex = 4
        Label1.Text = "StudentID"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(562, 54)
        Label2.Name = "Label2"
        Label2.Size = New Size(49, 20)
        Label2.TabIndex = 4
        Label2.Text = "Name"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(557, 96)
        Label3.Name = "Label3"
        Label3.Size = New Size(54, 20)
        Label3.TabIndex = 4
        Label3.Text = "Course"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(566, 135)
        Label5.Name = "Label5"
        Label5.Size = New Size(41, 20)
        Label5.TabIndex = 4
        Label5.Text = "Date"
        ' 
        ' TextBox3
        ' 
        TextBox3.BackColor = SystemColors.HighlightText
        TextBox3.Enabled = False
        TextBox3.Location = New Point(847, 93)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(126, 27)
        TextBox3.TabIndex = 3
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(783, 96)
        Label6.Name = "Label6"
        Label6.Size = New Size(58, 20)
        Label6.TabIndex = 4
        Label6.Text = "Section"
        ' 
        ' TextBox6
        ' 
        TextBox6.BackColor = SystemColors.HighlightText
        TextBox6.Enabled = False
        TextBox6.Location = New Point(633, 172)
        TextBox6.Name = "TextBox6"
        TextBox6.Size = New Size(340, 27)
        TextBox6.TabIndex = 3
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(549, 175)
        Label4.Name = "Label4"
        Label4.Size = New Size(58, 20)
        Label4.TabIndex = 4
        Label4.Text = "Time In"
        ' 
        ' Label7
        ' 
        Label7.Font = New Font("Segoe UI", 28.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label7.Location = New Point(536, 214)
        Label7.Name = "Label7"
        Label7.Size = New Size(437, 224)
        Label7.TabIndex = 5
        Label7.TextAlign = ContentAlignment.MiddleCenter
        Label7.UseMnemonic = False
        ' 
        ' AttendanceForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1004, 450)
        Controls.Add(Label7)
        Controls.Add(Label4)
        Controls.Add(Label5)
        Controls.Add(Label2)
        Controls.Add(Label6)
        Controls.Add(Label3)
        Controls.Add(Label1)
        Controls.Add(TextBox3)
        Controls.Add(TextBox6)
        Controls.Add(TextBox5)
        Controls.Add(TextBox4)
        Controls.Add(TextBox2)
        Controls.Add(TextBox1)
        Controls.Add(Button2)
        Controls.Add(PictureBox1)
        Name = "AttendanceForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Form2"
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Button2 As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label7 As Label
End Class
