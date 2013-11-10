Imports Microsoft.Devices.Sensors
Imports Microsoft.Phone.Reactive
Imports Microsoft.Xna.Framework
Imports System.Threading

Partial Public Class MainPage
    Inherits PhoneApplicationPage

    ' Constructor
    Private _Accelerometer As Accelerometer
    Private _UseEmulation As Boolean = True

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not _UseEmulation Then
            _Accelerometer = New Accelerometer
            Dim accelerometerReadingAsObservable = Observable.FromEvent(Of AccelerometerReadingEventArgs)(_Accelerometer, "ReadingChanged")

            Dim vector3FromAccelerometerEventArgs = From args In accelerometerReadingAsObservable
                                                    Select New Vector3(
                                                        CSng(args.EventArgs.X),
                                                        CSng(args.EventArgs.Y),
                                                        CSng(args.EventArgs.Z))

            vector3FromAccelerometerEventArgs.Subscribe(
                Sub(args)
                    InvokeAccelerometerReadingChanged(args)
                End Sub)

            Try
                _Accelerometer.Start()
            Catch ex As Exception
                ReadingTextBlock.Text = "Error starting accelerometer"
            End Try
        Else
            StartAccelerometerEmulation()
        End If
    End Sub

    Private _Subscribed As IDisposable
    Private Sub InvokeAccelerometerReadingChanged(ByVal data As Vector3)
        Deployment.Current.Dispatcher.BeginInvoke(
            Sub() AccelerometerReadingChanged(data))
    End Sub

    Private Sub StartAccelerometerEmulation()

        If _Subscribed Is Nothing Then
            Dim emulationObservable = GetEmulator()
            _Subscribed = emulationObservable.SubscribeOn(Scheduler.ThreadPool).
                Subscribe(Sub(accelerometerReadingEventArgs) InvokeAccelerometerReadingChanged(accelerometerReadingEventArgs))
        Else
            _Subscribed.Dispose()
            _Subscribed = Nothing
        End If
    End Sub

    Private Sub AccelerometerReadingChanged(ByVal data As Vector3)
        ReadingTextBlock.Text = String.Format("X: {0} y: {1} z: {2}",
                                              data.X.ToString("0.00"),
                                              data.Y.ToString("0.00"),
                                              data.Z.ToString("0.00"))
        Dim multiplier As Short
        If Short.TryParse(Me.MoveMultiplier.Text, multiplier) Then
            ButtonTransform.X = data.X * multiplier
            ButtonTransform.Y = data.Y * multiplier
        End If
    End Sub

    Public Shared Function GetEmulator() As IObservable(Of Vector3)
        Dim obs = Observable.Create(Of Vector3)(
            Function(subscriber)
                Dim rnd = New Random
                Dim theta As Double
                Do
                    theta = rnd.NextDouble
                    Dim reading = New Vector3(CSng(Math.Sin(theta)), CSng(Math.Cos(theta * 1.1)), CSng(Math.Sin(theta * 0.7)))
                    reading.Normalize()
                    Thread.Sleep(100)
                    subscriber.OnNext(reading)
                Loop
            End Function)
        Return obs

    End Function

End Class
