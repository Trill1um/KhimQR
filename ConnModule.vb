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

            InitializeDatabaseSchema()
        Catch ex As Exception
            MsgBox("Error in connection: " & ex.Message, MsgBoxStyle.Exclamation, "System Message:")
        End Try
    End Sub

    Private Sub InitializeDatabaseSchema()
        Try
            Dim sql = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Sql", "001_Create_KhimQR_Tables.sql"))
            Using cmd As New SqliteCommand(sql, sqlconn)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Schema Init", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub
End Module