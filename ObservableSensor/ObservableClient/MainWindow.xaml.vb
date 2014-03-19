Option Strict On

Imports Sensor
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.Reactive.Linq

Class MainWindow

    Private Sensor As New ObservableSensor

    Private Sub Start_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Start.Click
        Dim worker As New BackgroundWorker
        AddHandler worker.DoWork, Sub(s As Object, ars As DoWorkEventArgs)
                                      Sensor.StartSensor()
                                  End Sub
        worker.RunWorkerAsync(Sensor)
    End Sub

    Private Sub Stop_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles [Stop].Click
        Sensor.StopSensor()
    End Sub

    Private ActiveLowValueSensors As IDisposable

    Private Sub FilterLowValue_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles FilterLowValue.Click
        If ActiveLowValueSensors Is Nothing Then
            Dim lowValueSensors = From s In Sensor
                                  Where s.SensorValue < 3
                                  Select s.SensorValue

            ActiveLowValueSensors = lowValueSensors.Subscribe(New ConsoleObserver(Of Double)(ConsoleColor.Red))
        Else
            ActiveLowValueSensors.Dispose()
            ActiveLowValueSensors = Nothing
        End If

    End Sub

    Private Sub FilterFirstType_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles FilterFirstType.Click
        Dim items = New ObservableCollection(Of Double)
        FilteredList.ItemsSource = items

        Dim TypeSensors = From s In Sensor
                       Where s.SensorType = "4"
                       Select s.SensorValue

        TypeSensors.ObserveOnDispatcher.Subscribe(
            Sub(item) items.Add(item))

    End Sub

    Private Sub QueryAny_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles QueryAny.Click
        Dim AnySensor = Sensor.Any(
            Function(s) s.SensorValue > 17)
        AnySensor.Subscribe(Sub(s) MessageBox.Show(s.ToString, "OutOfRange"))

    End Sub

    Private Sub Heartbeat_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Heartbeat.Click

        Dim heartbeatAggregate = Sensor.
                                Where(Function(reading) reading.SensorType = "2").
                                Buffer(TimeSpan.FromSeconds(3))

        heartbeatAggregate.ObserveOnDispatcher.Subscribe(Sub(group) Heartbeat.Content = group.Count)

    End Sub

    Private Sub MouseMoveSamples_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MouseMoveSamples.Click
        Dim mouseSample = New MouseMoveSample
        mouseSample.Show()
    End Sub

    Private Sub FileWatcherButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles FileWatcherButton.Click
        Dim frm = New FileWatcher
        frm.Show()
    End Sub
End Class
