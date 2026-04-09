<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        TextBox5 = New TextBox()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label6 = New Label()
        PictureBox1 = New PictureBox()
        Button1 = New Button()
        Button2 = New Button()
        Button3 = New Button()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(181, 43)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(414, 27)
        TextBox1.TabIndex = 0
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(180, 86)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(415, 27)
        TextBox2.TabIndex = 1
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(181, 172)
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(415, 27)
        TextBox3.TabIndex = 3
        ' 
        ' TextBox4
        ' 
        TextBox4.Location = New Point(181, 129)
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(415, 27)
        TextBox4.TabIndex = 2
        ' 
        ' ComboBox1
        ' 
        ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBox1.FormattingEnabled = True
        ComboBox1.Location = New Point(180, 215)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(415, 28)
        ComboBox1.TabIndex = 4
        ' 
        ' TextBox5
        ' 
        TextBox5.Location = New Point(180, 259)
        TextBox5.Name = "TextBox5"
        TextBox5.Size = New Size(415, 27)
        TextBox5.TabIndex = 5
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(25, 45)
        Label1.Name = "Label1"
        Label1.Size = New Size(75, 20)
        Label1.TabIndex = 6
        Label1.Text = "StudentID"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(25, 89)
        Label2.Name = "Label2"
        Label2.Size = New Size(76, 20)
        Label2.TabIndex = 7
        Label2.Text = "FirstName"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(25, 132)
        Label3.Name = "Label3"
        Label3.Size = New Size(96, 20)
        Label3.TabIndex = 6
        Label3.Text = "MiddleName"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(26, 175)
        Label4.Name = "Label4"
        Label4.Size = New Size(75, 20)
        Label4.TabIndex = 7
        Label4.Text = "LastName"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(26, 218)
        Label5.Name = "Label5"
        Label5.Size = New Size(54, 20)
        Label5.TabIndex = 6
        Label5.Text = "Course"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(25, 262)
        Label6.Name = "Label6"
        Label6.Size = New Size(58, 20)
        Label6.TabIndex = 7
        Label6.Text = "Section"
        ' 
        ' PictureBox1
        ' 
        PictureBox1.BackColor = SystemColors.ButtonShadow
        PictureBox1.BackgroundImageLayout = ImageLayout.Zoom
        PictureBox1.Location = New Point(623, 47)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(257, 257)
        PictureBox1.SizeMode = PictureBoxSizeMode.CenterImage
        PictureBox1.TabIndex = 8
        PictureBox1.TabStop = False
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(623, 13)
        Button1.Name = "Button1"
        Button1.Size = New Size(257, 29)
        Button1.TabIndex = 9
        Button1.Text = "Generate QR Code"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(812, 318)
        Button2.Name = "Button2"
        Button2.Size = New Size(68, 29)
        Button2.TabIndex = 10
        Button2.Text = "Export"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(623, 318)
        Button3.Name = "Button3"
        Button3.Size = New Size(183, 29)
        Button3.TabIndex = 10
        Button3.Text = "SAVE DATA"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        AutoSizeMode = AutoSizeMode.GrowAndShrink
        ClientSize = New Size(910, 359)
        Controls.Add(Button3)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Controls.Add(PictureBox1)
        Controls.Add(Label6)
        Controls.Add(Label4)
        Controls.Add(Label2)
        Controls.Add(Label5)
        Controls.Add(Label3)
        Controls.Add(Label1)
        Controls.Add(TextBox5)
        Controls.Add(ComboBox1)
        Controls.Add(TextBox3)
        Controls.Add(TextBox4)
        Controls.Add(TextBox2)
        Controls.Add(TextBox1)
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Form1"
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button

End Class
