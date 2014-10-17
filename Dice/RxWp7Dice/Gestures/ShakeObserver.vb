Imports Microsoft.Phone.Reactive
Imports Microsoft.Devices.Sensors

Public Module ShakeObserver
    Const MinimumOffset = 1.44
    Const TimeThreshold = 200

    Public Function GetShakeObserver(ByVal accel As Accelerometer) As IObservable(Of Unit)
        Dim query = From start In Observable.FromEvent(Of AccelerometerReadingEventArgs)(accel, "ReadingChanged")
                    Where (start.EventArgs.X ^ 2 + start.EventArgs.Y ^ 2) > MinimumOffset
                    Select start

        Dim shake = From interval In query.TimeInterval
                    Where interval.Interval.TotalMilliseconds < MinimumOffset
                    Select New Unit

        Return shake
    End Function

    'Public Function GetShakeObserver(ByVal accel As Accelerometer) As IObservable(Of Unit)
    '    Dim query =
    '        From interval In
    '        (From startEvent In Observable.FromEvent(Of AccelerometerReadingEventArgs)(accel, "ReadingChanged")
    '          Where (startEvent.EventArgs.X * startEvent.EventArgs.X +
    '                startEvent.EventArgs.Y * startEvent.EventArgs.Y) > MinimumOffset
    '          Select startEvent).TimeInterval
    '        Where interval.Interval.TotalMilliseconds < TimeThreshold
    '        Select New Unit

    '    Return query

    'End Function
End Module