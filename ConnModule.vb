Imports Microsoft.Data.SqlClient
Imports System.IO

Module ConnModule
    Public startpath As String = Application.StartupPath
    Public SR As StreamReader
    Public srvr As String
    Public loc As String = (startpath + "/Connection, ini")
    Public sqlconn As New SqlConnection
    Public cmd As New SqlCommand
    Public dr As SqlDataReader
    Public da As New SqlDataAdapter
    Public ds As New DataSet
    Public dt As New DataTable
    Public str As String
    Public query As String
    Public USERID, PWORD, USERNAME, usertype, fname, mname, lname As String

    Sub connect()
        If Not File.Exists(loc) Then
            MsgBox("Server Location not found", MsgBoxStyle.Exclamation, "System Message:")
        Else
            Try
                SR = File.OpenText(loc)
                While SR.Peek <> -1
                    srvr = SR.ReadLine()

                End While
                SR.Close()
                If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
                sqlconn.ConnectionString = srvr + ";Trusted_Connection=True;MultipleActiveResultSets=true;"

                sqlconn.Open()
            Catch ex As Exception
                MsgBox("Error in connection, please contact the Administrator!", MsgBoxStyle.Exclamation, "System Message:")
                End
            End Try
        End If
    End Sub
End Module