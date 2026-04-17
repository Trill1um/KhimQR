<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        logoutButton = New Button()
        currentUserLabel = New Label()
        currentPageLabel = New Label()
        homeButton = New Button()
        pageHostPanel = New Panel()
        topPanel.SuspendLayout()
        SuspendLayout()
        ' 
        ' topPanel
        ' 
        topPanel.Controls.Add(logoutButton)
        topPanel.Controls.Add(currentUserLabel)
        topPanel.Controls.Add(currentPageLabel)
        topPanel.Controls.Add(homeButton)
        topPanel.Dock = DockStyle.Top
        topPanel.Location = New Point(0, 0)
        topPanel.Name = "topPanel"
        topPanel.Size = New Size(1004, 56)
        topPanel.TabIndex = 0
        ' 
        ' logoutButton
        ' 
        logoutButton.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        logoutButton.Location = New Point(898, 12)
        logoutButton.Name = "logoutButton"
        logoutButton.Size = New Size(94, 32)
        logoutButton.TabIndex = 2
        logoutButton.Text = "Logout"
        logoutButton.UseVisualStyleBackColor = True
        logoutButton.Visible = False
        ' 
        ' currentUserLabel
        ' 
        currentUserLabel.AutoSize = True
        currentUserLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        currentUserLabel.Location = New Point(407, 14)
        currentUserLabel.Name = "currentUserLabel"
        currentUserLabel.Size = New Size(193, 28)
        currentUserLabel.TabIndex = 2
        currentUserLabel.Text = "Logged in as: None"
        ' 
        ' currentPageLabel
        ' 
        currentPageLabel.AutoSize = True
        currentPageLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        currentPageLabel.Location = New Point(124, 14)
        currentPageLabel.Name = "currentPageLabel"
        currentPageLabel.Size = New Size(68, 28)
        currentPageLabel.TabIndex = 1
        currentPageLabel.Text = "Home"
        ' 
        ' homeButton
        ' 
        homeButton.Location = New Point(12, 12)
        homeButton.Name = "homeButton"
        homeButton.Size = New Size(94, 32)
        homeButton.TabIndex = 0
        homeButton.Text = "Home"
        homeButton.UseVisualStyleBackColor = True
        ' 
        ' pageHostPanel
        ' 
        pageHostPanel.Dock = DockStyle.Fill
        pageHostPanel.Location = New Point(0, 56)
        pageHostPanel.Name = "pageHostPanel"
        pageHostPanel.Size = New Size(1004, 450)
        pageHostPanel.TabIndex = 1
        ' 
        ' MainForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1004, 506)
        Controls.Add(pageHostPanel)
        Controls.Add(topPanel)
        Name = "MainForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "KhimQR"
        topPanel.ResumeLayout(False)
        topPanel.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents topPanel As Panel
    Friend WithEvents currentPageLabel As Label
    Friend WithEvents currentUserLabel As Label
    Friend WithEvents homeButton As Button
    Friend WithEvents logoutButton As Button
    Friend WithEvents pageHostPanel As Panel
End Class
