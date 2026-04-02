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
            Using cmd As New SqliteCommand()
                cmd.Connection = sqlconn

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Autonumber (" &
                                 "pfx TEXT PRIMARY KEY, " &
                                 "NewNumber TEXT NOT NULL" &
                                 ")"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Course (" &
                                 "Code TEXT PRIMARY KEY, " &
                                 "Name TEXT NOT NULL" &
                                 ")"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS StudentMasterLists (" &
                                 "StudentID TEXT PRIMARY KEY, " &
                                 "Firstname TEXT, " &
                                 "Middlename TEXT, " &
                                 "Lastname TEXT, " &
                                 "Course TEXT, " &
                                 "Section TEXT, " &
                                 "QRCode BLOB" &
                                 ")"
                cmd.ExecuteNonQuery()

                cmd.CommandText = "CREATE TABLE IF NOT EXISTS Attendance (" &
                                 "RecNumber INTEGER PRIMARY KEY AUTOINCREMENT, " &
                                 "StudentID TEXT NOT NULL, " &
                                 "Date_STAMP TEXT NOT NULL, " &
                                 "TimeIN TEXT NOT NULL" &
                                 ")"
                cmd.ExecuteNonQuery()
            End Using
        Catch
        End Try
    End Sub
End Module