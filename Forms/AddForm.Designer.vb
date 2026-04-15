<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AddForm
    Inherits System.Windows.Forms.Form

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        tableLabel = New Label()
        entityCombo = New ComboBox()
        fieldsPanel = New FlowLayoutPanel()
        saveButton = New Button()
        SuspendLayout()
        ' 
        ' tableLabel
        ' 
        tableLabel.AutoSize = True
        tableLabel.Location = New Point(20, 20)
        tableLabel.Name = "tableLabel"
        tableLabel.Size = New Size(44, 20)
        tableLabel.TabIndex = 0
        tableLabel.Text = "Table"
        ' 
        ' entityCombo
        ' 
        entityCombo.DropDownStyle = ComboBoxStyle.DropDownList
        entityCombo.FormattingEnabled = True
        entityCombo.Location = New Point(20, 45)
        entityCombo.Name = "entityCombo"
        entityCombo.Size = New Size(500, 28)
        entityCombo.TabIndex = 1
        ' 
        ' fieldsPanel
        ' 
        fieldsPanel.AutoScroll = True
        fieldsPanel.FlowDirection = FlowDirection.TopDown
        fieldsPanel.Location = New Point(20, 85)
        fieldsPanel.Name = "fieldsPanel"
        fieldsPanel.Size = New Size(500, 310)
        fieldsPanel.TabIndex = 2
        fieldsPanel.WrapContents = False
        ' 
        ' saveButton
        ' 
        saveButton.Location = New Point(420, 410)
        saveButton.Name = "saveButton"
        saveButton.Size = New Size(100, 29)
        saveButton.TabIndex = 3
        saveButton.Text = "Save"
        saveButton.UseVisualStyleBackColor = True
        ' 
        ' AddForm
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1004, 450)
        Controls.Add(saveButton)
        Controls.Add(fieldsPanel)
        Controls.Add(entityCombo)
        Controls.Add(tableLabel)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "AddForm"
        StartPosition = FormStartPosition.CenterParent
        Text = "Add Data"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents tableLabel As Label
    Friend WithEvents entityCombo As ComboBox
    Friend WithEvents fieldsPanel As FlowLayoutPanel
    Friend WithEvents saveButton As Button
End Class
