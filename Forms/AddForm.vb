Imports Microsoft.Data.Sqlite
Imports System.Data

Partial Class AddForm
    Private ReadOnly controlsByField As New Dictionary(Of String, Control)(StringComparer.OrdinalIgnoreCase)

    Private ReadOnly studentRepository As New StudentRepository()
    Private ReadOnly professorRepository As New ProfessorRepository()
    Private ReadOnly courseRepository As New CourseRepository()
    Private ReadOnly classSectionRepository As New ClassSectionRepository()
    Private ReadOnly classSessionRepository As New ClassSessionRepository()

    Public Sub New()
        InitializeComponent()

        entityCombo.Items.AddRange(New Object() {"Student", "Professor", "Course", "ClassSection", "ClassSession"})
        entityCombo.SelectedIndex = 0
    End Sub

    Private Sub entityCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles entityCombo.SelectedIndexChanged
        RenderFields(entityCombo.SelectedItem.ToString())
    End Sub

    Private Sub RenderFields(entity As String)
        fieldsPanel.Controls.Clear()
        controlsByField.Clear()

        Select Case entity
            Case "Student"
                AddField("Student_Code")
                AddField("FirstName")
                AddField("MiddleName")
                AddField("LastName")
            Case "Professor"
                AddField("FirstName")
                AddField("MiddleName")
                AddField("LastName")
                AddField("Password", passwordChar:=True)
            Case "Course"
                AddField("Code")
                AddField("Name")
            Case "ClassSection"
                AddField("Course")
                AddField("Professor_ID")
                AddField("SectionName", "", True)
                AddField("GracePeriodMinutes", "15")
            Case "ClassSession"
                AddField("Course")
                AddField("SectionName", "", False)
                AddField("DayOfWeek")
                AddField("StartTime (HH:mm)")
                AddField("EndTime (HH:mm)")
        End Select
    End Sub

    Private Sub AddField(caption As String, Optional defaultValue As String = "", Optional allowTypedSectionName As Boolean = False, Optional passwordChar As Boolean = False)
        Dim row As New Panel() With {.Width = 470, .Height = 56}

        Dim label As New Label() With {
            .Text = caption,
            .Left = 0,
            .Top = 0,
            .Width = 460
        }

        Dim key = caption.Replace(" (HH:mm)", "")
        Dim inputControl As Control

        If key.Equals("StartTime", StringComparison.OrdinalIgnoreCase) OrElse
           key.Equals("EndTime", StringComparison.OrdinalIgnoreCase) Then
            Dim timePicker As New DateTimePicker() With {
                .Left = 0,
                .Top = 22,
                .Width = 460,
                .Format = DateTimePickerFormat.Custom,
                .CustomFormat = "HH:mm",
                .ShowUpDown = True
            }

            If Not String.IsNullOrWhiteSpace(defaultValue) Then
                Dim parsed As DateTime
                If DateTime.TryParse(defaultValue, parsed) Then
                    timePicker.Value = parsed
                End If
            End If

            inputControl = timePicker
        ElseIf key.Equals("DayOfWeek", StringComparison.OrdinalIgnoreCase) Then
            Dim dayTable As New DataTable()
            dayTable.Columns.Add("Value", GetType(Integer))
            dayTable.Columns.Add("Text", GetType(String))
            dayTable.Rows.Add(0, "Sunday")
            dayTable.Rows.Add(1, "Monday")
            dayTable.Rows.Add(2, "Tuesday")
            dayTable.Rows.Add(3, "Wednesday")
            dayTable.Rows.Add(4, "Thursday")
            dayTable.Rows.Add(5, "Friday")
            dayTable.Rows.Add(6, "Saturday")

            inputControl = New ComboBox() With {
                .Left = 0,
                .Top = 22,
                .Width = 460,
                .DropDownStyle = ComboBoxStyle.DropDownList,
                .DataSource = dayTable,
                .DisplayMember = "Text",
                .ValueMember = "Value"
            }
        ElseIf key.Equals("Course", StringComparison.OrdinalIgnoreCase) OrElse key.Equals("Course_ID", StringComparison.OrdinalIgnoreCase) Then
            inputControl = CreateLookupCombo(
                "SELECT Course_ID, Code || ' - ' || Name AS DisplayText FROM Course ORDER BY Code",
                "DisplayText",
                "Course_ID")
        ElseIf key.Equals("Professor_ID", StringComparison.OrdinalIgnoreCase) Then
            inputControl = CreateLookupCombo(
                "SELECT Professor_ID, FirstName || ' ' || CASE WHEN IFNULL(TRIM(MiddleName), '') = '' THEN '' ELSE SUBSTR(TRIM(MiddleName), 1, 1) || '. ' END || LastName AS DisplayText FROM Professor ORDER BY LastName, FirstName",
                "DisplayText",
                "Professor_ID")
        ElseIf key.Equals("SectionName", StringComparison.OrdinalIgnoreCase) Then
            inputControl = CreateLookupCombo(
                "SELECT DISTINCT SectionName AS DisplayText, SectionName FROM ClassSection ORDER BY SectionName",
                "DisplayText",
                "SectionName",
                If(allowTypedSectionName, ComboBoxStyle.DropDown, ComboBoxStyle.DropDownList))
        Else
            Dim textBox As New TextBox() With {
                .Left = 0,
                .Top = 22,
                .Width = 460,
                .Text = defaultValue
            }
            If passwordChar Then
                textBox.UseSystemPasswordChar = True
            End If
            inputControl = textBox
        End If

        row.Controls.Add(label)
        row.Controls.Add(inputControl)
        fieldsPanel.Controls.Add(row)

        controlsByField(key) = inputControl
    End Sub

    Private Function CreateLookupCombo(sql As String, displayMember As String, valueMember As String, Optional style As ComboBoxStyle = ComboBoxStyle.DropDownList) As ComboBox
        If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
            connect()
        End If

        Dim table As New DataTable()
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
        End Using

        Return New ComboBox() With {
            .Left = 0,
            .Top = 22,
            .Width = 460,
            .DropDownStyle = style,
            .DataSource = table,
            .DisplayMember = displayMember,
            .ValueMember = valueMember
        }
    End Function

    Private Sub saveButton_Click(sender As Object, e As EventArgs) Handles saveButton.Click
        Try
            If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
                connect()
            End If

            Select Case entityCombo.SelectedItem.ToString()
                Case "Student"
                    studentRepository.Save(sqlconn, New Student With {
                        .Student_Code = GetRequired("Student_Code"),
                        .FirstName = GetRequired("FirstName"),
                        .MiddleName = GetOptional("MiddleName"),
                        .LastName = GetRequired("LastName")
                    })

                Case "Professor"
                    professorRepository.Save(sqlconn, New Professor With {
                        .FirstName = GetRequired("FirstName"),
                        .MiddleName = GetOptional("MiddleName"),
                        .LastName = GetRequired("LastName"),
                        .Password = GetRequired("Password")
                    })

                Case "Course"
                    courseRepository.Save(sqlconn, New Course With {
                        .Code = GetRequired("Code"),
                        .Name = GetRequired("Name")
                    })

                Case "ClassSection"
                    Dim courseId = GetInt("Course")
                    Dim sectionName = GetRequired("SectionName")

                    If ClassSectionComboExists(courseId, sectionName) Then
                        MessageBox.Show("That Course_ID and SectionName combination already exists.", "Add", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If

                    Dim grace = GetInt("GracePeriodMinutes")
                    Dim professorId = GetInt("Professor_ID")

                    classSectionRepository.Save(sqlconn, New ClassSection With {
                        .Course_ID = courseId,
                        .Professor_ID = professorId,
                        .SectionName = sectionName,
                        .GracePeriodMinutes = grace
                    })

                Case "ClassSession"
                    Dim courseId = GetInt("Course")
                    Dim sectionName = GetRequired("SectionName")
                    Dim classSectionId = GetClassSectionId(courseId, sectionName)

                    classSessionRepository.Save(sqlconn, New ClassSession With {
                        .ClassSection_ID = classSectionId,
                        .DayOfWeek = GetInt("DayOfWeek"),
                        .StartTime = GetRequired("StartTime"),
                        .EndTime = GetRequired("EndTime")
                    })
            End Select

            MessageBox.Show("Saved successfully.", "Add", MessageBoxButtons.OK, MessageBoxIcon.Information)
            RenderFields(entityCombo.SelectedItem.ToString())
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Add", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Function ClassSectionComboExists(courseId As Integer, sectionName As String) As Boolean
        Using cmd As New SqliteCommand("SELECT 1 FROM ClassSection WHERE Course_ID = @c AND SectionName = @s LIMIT 1", sqlconn)
            cmd.Parameters.AddWithValue("@c", courseId)
            cmd.Parameters.AddWithValue("@s", sectionName)
            Dim res = cmd.ExecuteScalar()
            Return res IsNot Nothing AndAlso Not DBNull.Value.Equals(res)
        End Using
    End Function

    Private Function GetRequired(field As String) As String
        Dim control = controlsByField(field)

        If TypeOf control Is DateTimePicker Then
            Return DirectCast(control, DateTimePicker).Value.ToString("HH:mm")
        End If

        If TypeOf control Is ComboBox Then
            Dim combo = DirectCast(control, ComboBox)
            If combo.SelectedItem Is Nothing Then
                Throw New InvalidOperationException($"{field} is required.")
            End If
            Return combo.Text.Trim()
        End If

        Dim value = DirectCast(control, TextBox).Text.Trim()
        If String.IsNullOrWhiteSpace(value) Then
            Throw New InvalidOperationException($"{field} is required.")
        End If
        Return value
    End Function

    Private Function GetOptional(field As String) As String
        Dim control = controlsByField(field)
        If TypeOf control Is DateTimePicker Then
            Return DirectCast(control, DateTimePicker).Value.ToString("HH:mm")
        End If
        If TypeOf control Is ComboBox Then
            Return DirectCast(control, ComboBox).Text.Trim()
        End If
        Return DirectCast(control, TextBox).Text.Trim()
    End Function

    Private Function GetInt(field As String) As Integer
        Dim control = controlsByField(field)

        If TypeOf control Is ComboBox Then
            Dim selectedValue = DirectCast(control, ComboBox).SelectedValue
            If selectedValue Is Nothing OrElse DBNull.Value.Equals(selectedValue) Then
                Throw New InvalidOperationException($"{field} is required.")
            End If
            Return Convert.ToInt32(selectedValue)
        End If

        Dim value = GetRequired(field)
        Dim parsed As Integer
        If Not Integer.TryParse(value, parsed) Then
            Throw New InvalidOperationException($"{field} must be a valid number.")
        End If
        Return parsed
    End Function

    Private Function GetClassSectionId(courseId As Integer, sectionName As String) As Integer
        Using cmd As New SqliteCommand("SELECT ClassSection_ID FROM ClassSection WHERE Course_ID = @c AND SectionName = @s LIMIT 1", sqlconn)
            cmd.Parameters.AddWithValue("@c", courseId)
            cmd.Parameters.AddWithValue("@s", sectionName)
            Dim res = cmd.ExecuteScalar()
            If res Is Nothing OrElse DBNull.Value.Equals(res) Then
                Throw New InvalidOperationException($"The class section '{sectionName}' for the selected course does not exist.")
            End If
            Return Convert.ToInt32(res)
        End Using
    End Function
End Class
