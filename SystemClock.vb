Public Class SystemClock
    Public Shared SimulatedTime As DateTime? = Nothing

    Public Shared ReadOnly Property Now As DateTime
        Get
            Return If(SimulatedTime.HasValue, SimulatedTime.Value, DateTime.Now)
        End Get
    End Property

    Public Shared ReadOnly Property Today As DateTime
        Get
            Return Now.Date
        End Get
    End Property
End Class