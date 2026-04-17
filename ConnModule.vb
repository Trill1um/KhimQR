Imports Microsoft.Data.Sqlite
Imports System.IO

Module ConnModule
    Public startpath As String = Application.StartupPath
    Public dbPath As String = Path.Combine(startpath, "KhimQR.db")
    Public sqlconn As New SqliteConnection

    Sub connect()
        Try
            Dim connectionString = $"Data Source={dbPath};Mode=ReadWriteCreate;Cache=Shared"

            If sqlconn.State = ConnectionState.Open Then
                sqlconn.Close()
            End If

            sqlconn.ConnectionString = connectionString
            sqlconn.Open()

            EnsureDatabaseReady()
        Catch ex As Exception
            MsgBox("Error in connection: " & ex.Message, MsgBoxStyle.Exclamation, "System Message:")
        End Try
    End Sub

    Public Sub ResetAndInitializeDatabase()
        Try
            If sqlconn Is Nothing OrElse sqlconn.State <> ConnectionState.Open Then
                connect()
            End If

            ExecuteSqlFile("999_Reset_KhimQR_Database.sql")
            EnsureDatabaseReady()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Reset DB", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub EnsureDatabaseReady()
        Try
            If Not IsSchemaReady() Then
                ExecuteSqlFile("001_Create_KhimQR_Tables.sql")
            End If

            EnsureEnrollmentDateColumn()
            EnsureProfessorSecurityColumns()

            If Not HasSeedData() Then
                ExecuteSqlFile("002_Seed_KhimQR_Data.sql")
            End If

            NormalizeSeededMiddleNames()
            EnsureAdminProfessorAccount()
            EnsureProfessorPasswords()
            EnsureStudentQrCodes()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Schema Init", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub EnsureEnrollmentDateColumn()
        Const columnCheckSql As String = "SELECT COUNT(1) FROM pragma_table_info('Enrollment') WHERE name = 'EnrollmentDate'"
        Using cmd As New SqliteCommand(columnCheckSql, sqlconn)
            If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                Return
            End If
        End Using

        Using cmd As New SqliteCommand("ALTER TABLE Enrollment ADD COLUMN EnrollmentDate TEXT", sqlconn)
            cmd.ExecuteNonQuery()
        End Using

        Using cmd As New SqliteCommand("UPDATE Enrollment SET EnrollmentDate = COALESCE(EnrollmentDate, date('2000-01-01')) WHERE EnrollmentDate IS NULL OR TRIM(EnrollmentDate) = ''", sqlconn)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub EnsureProfessorSecurityColumns()
        EnsureProfessorColumn("Password", "TEXT")
        EnsureProfessorColumn("IsAdmin", "INTEGER NOT NULL DEFAULT 0")
    End Sub

    Private Sub EnsureProfessorColumn(columnName As String, columnDefinition As String)
        Dim columnCheckSql = $"SELECT COUNT(1) FROM pragma_table_info('Professor') WHERE name = '{columnName}'"
        Using cmd As New SqliteCommand(columnCheckSql, sqlconn)
            If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                Return
            End If
        End Using

        Using cmd As New SqliteCommand($"ALTER TABLE Professor ADD COLUMN {columnName} {columnDefinition}", sqlconn)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub NormalizeSeededMiddleNames()
        If Not IsSchemaReady() Then
            Return
        End If

        Dim professorUpdates As String() = {
            "UPDATE Professor SET MiddleName = 'Alexander' WHERE Professor_ID = 1 AND MiddleName IN ('A.', 'A')",
            "UPDATE Professor SET MiddleName = 'Carter' WHERE Professor_ID = 2 AND MiddleName IN ('C.', 'C')",
            "UPDATE Professor SET MiddleName = 'Miguel' WHERE Professor_ID = 3 AND MiddleName IN ('M.', 'M')",
            "UPDATE Professor SET MiddleName = 'Patricia' WHERE Professor_ID = 4 AND MiddleName IN ('P.', 'P')",
            "UPDATE Professor SET MiddleName = 'Luis' WHERE Professor_ID = 5 AND MiddleName IN ('L.', 'L')"
        }

        For Each sql In professorUpdates
            Using cmd As New SqliteCommand(sql, sqlconn)
                cmd.ExecuteNonQuery()
            End Using
        Next

        Using cmd As New SqliteCommand("UPDATE Student SET MiddleName = CASE MiddleName WHEN 'A.' THEN 'Anderson' WHEN 'A' THEN 'Anderson' WHEN 'B.' THEN 'Bernard' WHEN 'B' THEN 'Bernard' WHEN 'C.' THEN 'Carter' WHEN 'C' THEN 'Carter' WHEN 'D.' THEN 'Daniel' WHEN 'D' THEN 'Daniel' WHEN 'E.' THEN 'Edward' WHEN 'E' THEN 'Edward' ELSE MiddleName END WHERE MiddleName IN ('A.', 'A', 'B.', 'B', 'C.', 'C', 'D.', 'D', 'E.', 'E')", sqlconn)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub EnsureProfessorPasswords()
        If Not IsSchemaReady() Then
            Return
        End If

        Dim updateSql As String = "UPDATE Professor SET Password = COALESCE(NULLIF(TRIM(Password), ''), FirstName || '123') WHERE Password IS NULL OR TRIM(Password) = ''"
        Using cmd As New SqliteCommand(updateSql, sqlconn)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub EnsureAdminProfessorAccount()
        If Not IsSchemaReady() Then
            Return
        End If

        Const adminCheckSql As String = "SELECT COUNT(1) FROM Professor WHERE IsAdmin = 1 OR LOWER(FirstName) = 'admin'"
        Using cmd As New SqliteCommand(adminCheckSql, sqlconn)
            If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                Using updateCmd As New SqliteCommand("UPDATE Professor SET IsAdmin = 1, Password = COALESCE(NULLIF(TRIM(Password), ''), 'Admin123') WHERE IsAdmin = 1 OR LOWER(FirstName) = 'admin'", sqlconn)
                    updateCmd.ExecuteNonQuery()
                End Using
                Return
            End If
        End Using

        Const insertSql As String = "INSERT OR IGNORE INTO Professor (Professor_ID, FirstName, MiddleName, LastName, Password, IsAdmin) VALUES (6, 'Admin', NULL, 'Account', 'Admin123', 1)"
        Using cmd As New SqliteCommand(insertSql, sqlconn)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Function IsSchemaReady() As Boolean
        Const sql As String = "SELECT COUNT(1) FROM sqlite_master WHERE type='table' AND name='Professor'"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
        End Using
    End Function

    Private Function HasSeedData() As Boolean
        If Not IsSchemaReady() Then
            Return False
        End If

        Const sql As String = "SELECT COUNT(1) FROM Professor"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
        End Using
    End Function

    Private Sub EnsureStudentQrCodes()
        If Not IsSchemaReady() Then
            Return
        End If

        Dim qrCodeService As New QrCodeService()
        Dim qrStorage As New QrStorageService()
        qrStorage.EnsureDefaultDirectory()

        Const sql As String = "SELECT Student_Code FROM Student WHERE Student_Code IS NOT NULL AND TRIM(Student_Code) <> ''"
        Using cmd As New SqliteCommand(sql, sqlconn)
            Using reader = cmd.ExecuteReader()
                While reader.Read()
                    Dim studentCode = reader("Student_Code").ToString().Trim()
                    If String.IsNullOrWhiteSpace(studentCode) Then
                        Continue While
                    End If

                    Dim filePath = Path.Combine(qrStorage.GetDefaultDirectory(), studentCode & ".png")
                    If File.Exists(filePath) Then
                        Continue While
                    End If

                    Using qrImage = qrCodeService.Generate(studentCode, 12)
                        qrStorage.SaveToDefaultDirectory(qrImage, studentCode)
                    End Using
                End While
            End Using
        End Using
    End Sub

    Private Sub ExecuteSqlFile(fileName As String)
        Dim sqlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Sql", fileName)
        Dim sql = File.ReadAllText(sqlPath)
        Using cmd As New SqliteCommand(sql, sqlconn)
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Module