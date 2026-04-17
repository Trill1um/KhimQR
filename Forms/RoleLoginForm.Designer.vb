<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RoleLoginForm
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
        roleComboBox = New ComboBox()
        passwordLabel = New Label()
        passwordTextBox = New TextBox()
        loginButton = New Button()
        SuspendLayout()
        ' 
        ' titleLabel
        ' 
        titleLabel.AutoSize = True
        titleLabel.Font = New Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        titleLabel.Location = New Point(333, 95)
        titleLabel.Name = "titleLabel"
        titleLabel.Size = New Size(336, 54)
        titleLabel.TabIndex = 0
        titleLabel.Text = "Login to continue"
        ' 
        ' roleComboBox
        ' 
        roleComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        roleComboBox.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        roleComboBox.FormattingEnabled = True
        roleComboBox.Location = New Point(344, 175)
        roleComboBox.Name = "roleComboBox"
        roleComboBox.Size = New Size(321, 36)
        roleComboBox.TabIndex = 1
        ' 
        ' passwordLabel
        ' 
        passwordLabel.AutoSize = True
        passwordLabel.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        passwordLabel.Location = New Point(344, 227)
        passwordLabel.Name = "passwordLabel"
        passwordLabel.Size = New Size(96, 28)
        passwordLabel.TabIndex = 2
        passwordLabel.Text = "Password"
        ' 
        ' passwordTextBox
        ' 
        passwordTextBox.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        passwordTextBox.Location = New Point(344, 258)
        passwordTextBox.Name = "passwordTextBox"
        passwordTextBox.Size = New Size(321, 34)
        passwordTextBox.TabIndex = 3
        passwordTextBox.UseSystemPasswordChar = True
        ' 
        ' loginButton
        ' 
        loginButton.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        loginButton.Location = New Point(422, 314)
        loginButton.Name = "loginButton"
        loginButton.Size = New Size(160, 46)
        loginButton.TabIndex = 4
        loginButton.Text = "Login"
        loginButton.UseVisualStyleBackColor = True
        ' 
        ' RoleLoginForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1004, 450)
        Controls.Add(loginButton)
        Controls.Add(passwordTextBox)
        Controls.Add(passwordLabel)
        Controls.Add(roleComboBox)
        Controls.Add(titleLabel)
        FormBorderStyle = FormBorderStyle.FixedSingle
        Name = "RoleLoginForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Login"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents titleLabel As Label
    Friend WithEvents roleComboBox As ComboBox
    Friend WithEvents passwordLabel As Label
    Friend WithEvents passwordTextBox As TextBox
    Friend WithEvents loginButton As Button
End Class