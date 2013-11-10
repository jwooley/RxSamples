Public Class SensorInfo
    Public Property TimeStamp As DateTime
    Public Property SensorType As String
    Public Property SensorValue As Double

    Public Overrides Function ToString() As String
        Return String.Format("Time: {0}  , Type: {1}  Value: {2}", TimeStamp, SensorType, SensorValue)
    End Function
End Class
