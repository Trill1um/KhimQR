Imports Microsoft.Data.Sqlite

Public Class AutoNumberRepository
    Public Function GetCurrentOrDefault(connection As SqliteConnection, prefix As String, numberWidth As Integer) As String
        Dim currentCode = GetCurrentCode(connection, prefix)
        If String.IsNullOrWhiteSpace(currentCode) Then
            Return BuildCode(prefix, 1, numberWidth)
        End If

        Return currentCode
    End Function

    Public Function GetCurrentOrCreate(connection As SqliteConnection, prefix As String, numberWidth As Integer) As String
        Dim currentCode = GetCurrentCode(connection, prefix)
        If String.IsNullOrWhiteSpace(currentCode) Then
            currentCode = BuildCode(prefix, 1, numberWidth)
            UpsertCurrentCode(connection, prefix, currentCode)
        End If

        Return currentCode
    End Function

    Public Function IncrementAndGet(connection As SqliteConnection, prefix As String, numberWidth As Integer) As String
        Dim currentCode = GetCurrentOrCreate(connection, prefix, numberWidth)
        Dim nextNumber = ExtractNumber(currentCode, prefix) + 1
        Dim nextCode = BuildCode(prefix, nextNumber, numberWidth)

        UpsertCurrentCode(connection, prefix, nextCode)

        Return nextCode
    End Function

    Private Function GetCurrentCode(connection As SqliteConnection, prefix As String) As String
        Const query As String = "SELECT MAX(NewNumber) FROM Autonumber WHERE pfx = @pfx"

        Using cmd As New SqliteCommand(query, connection)
            cmd.Parameters.AddWithValue("@pfx", prefix)
            Dim value = cmd.ExecuteScalar()

            If value Is Nothing OrElse value Is DBNull.Value Then
                Return String.Empty
            End If

            Return Convert.ToString(value)
        End Using
    End Function

    Private Sub UpsertCurrentCode(connection As SqliteConnection, prefix As String, code As String)
        Const upsertSql As String = "INSERT INTO Autonumber (pfx, NewNumber) VALUES (@pfx, @newNumber) " &
                                    "ON CONFLICT(pfx) DO UPDATE SET NewNumber = @newNumber"

        Using cmd As New SqliteCommand(upsertSql, connection)
            cmd.Parameters.AddWithValue("@pfx", prefix)
            cmd.Parameters.AddWithValue("@newNumber", code)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Function ExtractNumber(code As String, prefix As String) As Integer
        If String.IsNullOrWhiteSpace(code) Then
            Return 0
        End If

        Dim numericPart = code
        If numericPart.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) Then
            numericPart = numericPart.Substring(prefix.Length)
        End If

        numericPart = numericPart.TrimStart("-"c)

        Dim value As Integer
        If Integer.TryParse(numericPart, value) Then
            Return value
        End If

        Return 0
    End Function

    Private Function BuildCode(prefix As String, number As Integer, numberWidth As Integer) As String
        Return prefix & number.ToString("D" & numberWidth)
    End Function
End Class

