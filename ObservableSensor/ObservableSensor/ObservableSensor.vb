Imports System.Reactive.Linq

Public Class ObservableSensor
    Implements IObservable(Of SensorInfo)

    Private source As IObservable(Of SensorInfo)

    Private _running As Boolean
  
    Public Function Subscribe(ByVal observer As System.IObserver(Of SensorInfo)) As System.IDisposable Implements System.IObservable(Of SensorInfo).Subscribe
        _running = True
        Dim disposeHandle = source.Subscribe(observer)
        Return disposeHandle
    End Function

    Public Sub StartSensor()
        Dim randomizer = New Random(Date.Now.Millisecond)

        source = Observable.Generate(_running,
                    Function(x) _running,
                    Function(isRunning) isRunning,
                    Function(isRunning) New SensorInfo With {.SensorType = CInt(randomizer.NextDouble * 4).ToString,
                                                            .SensorValue = randomizer.NextDouble * 20,
                                                            .TimeStamp = Now},
                    Function(x) TimeSpan.FromMilliseconds(randomizer.NextDouble * 100))


    End Sub

    Public Sub StopSensor()
        _running = False
    End Sub

End Class
