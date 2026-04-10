Public Class Student
    Public Property ID As Integer
    Public Property Student_Code As String
    Public Property FirstName As String
    Public Property MiddleName As String
    Public Property LastName As String
End Class

Public Class Professor
    Public Property Professor_ID As Integer
    Public Property FirstName As String
    Public Property MiddleName As String
    Public Property LastName As String
End Class

Public Class Course
    Public Property Course_ID As Integer
    Public Property Code As String
    Public Property Name As String
End Class

Public Class ClassSection
    Public Property ClassSection_ID As Integer
    Public Property Course_ID As Integer
    Public Property Section_ID As Integer
    Public Property Professor_ID As Integer
    Public Property SectionName As String
    Public Property GracePeriodMinutes As Integer
End Class

Public Class ClassSession
    Public Property ClassSession_ID As Integer
    Public Property ClassSection_ID As Integer
    Public Property DayOfWeek As Integer
    Public Property StartTime As String
    Public Property EndTime As String
End Class

Public Class Enrollment
    Public Property Enrollment_ID As Integer
    Public Property ClassSection_ID As Integer
    Public Property Student_ID As Integer
End Class

Public Class Attendance
    Public Property Attendance_ID As Integer
    Public Property ClassSession_ID As Integer
    Public Property Enrollment_ID As Integer
    Public Property Date_Stamp As String
    Public Property TimeIn As String
    Public Property Status As String
End Class
