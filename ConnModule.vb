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

            If Not HasSeedData() Then
                ExecuteSqlFile("002_Seed_KhimQR_Data.sql")
            End If

            EnsureStudentQrCodes()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Schema Init", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
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