Imports Microsoft.Phone.Reactive
Imports Microsoft.Devices.Sensors

Public Module ShakeObserver
    Const MinimumOffset = 1.44
    Const TimeThreshold = 200

    Public Function GetShakeObserver(ByVal accel As Accelerometer) As IObservable(Of Unit)

        Dim Movements = From start In Observable.FromEvent(Of AccelerometerReadingEventArgs)(accel, "ReadingChanged")
                        Where (start.EventArgs.X ^ 2 + start.EventArgs.Y ^ 2) > MinimumOffset

        Dim shake = From movement In Movements.TimeInterval
                    Where movement.Interval.TotalMilliseconds < TimeThreshold
                    Select New Unit()

        Return shake.Throttle(TimeSpan.FromSeconds(1.5)).
            ObserveOnDispatcher()

    End Function

End Module