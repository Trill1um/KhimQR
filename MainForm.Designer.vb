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
        currentPageLabel = New Label()
        homeButton = New Button()
        pageHostPanel = New Panel()
        topPanel.SuspendLayout()
        SuspendLayout()
        ' 
        ' topPanel
        ' 
        topPanel.Controls.Add(currentPageLabel)
        topPanel.Controls.Add(homeButton)
        topPanel.Dock = DockStyle.Top
        topPanel.Location = New Point(0, 0)
        topPanel.Name = "topPanel"
        topPanel.Size = New Size(1100, 56)
        topPanel.TabIndex = 0
        ' 
        ' currentPageLabel
        ' 
        currentPageLabel.AutoSize = True
        currentPageLabel.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        currentPageLabel.Location = New Point(124, 14)
        currentPageLabel.Name = "currentPageLabel"
        currentPageLabel.Size = New Size(62, 28)
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
        pageHostPanel.Size = New Size(1100, 594)
        pageHostPanel.TabIndex = 1
        ' 
        ' MainForm
        ' 
        AutoScaleDimensions = New SizeF(8.0!, 20.0!)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1100, 650)
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
    Friend WithEvents homeButton As Button
    Friend WithEvents pageHostPanel As Panel
End Class
