using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;
using SilverlightApplication1.Views;
using SilverlightApplication1.Observables;

namespace SilverlightApplication1
{
    public partial class Sensor : Page
    {
        public Sensor()
        {
            InitializeComponent();
        }

        ObservableSensor sensor = new ObservableSensor();

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(_ => sensor.StartSensor());
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            sensor.StopSensor();
        }

        IDisposable activeLowValueSensor;

        private void FilterLowValue_Click(object sender, RoutedEventArgs e)
        {
            if (activeLowValueSensor == null)
            {
                var lowValueSensors = from s in sensor
                                      where s.SensorValue < 3
                                      select s.SensorValue;
                activeLowValueSensor = lowValueSensors.Subscribe(new ConsoleObserver<double>());
            }
            else
            {
                activeLowValueSensor.Dispose();
                activeLowValueSensor = null;
            }
        }

        private void FilterFirstType_Click(object sender, RoutedEventArgs e)
        {
            var listSource = new ObserverCollection<double>();

            var typeSensors = from s in sensor
                              where s.SensorType == "2"
                              select s.SensorValue;
          
            FilteredList.ItemsSource = listSource;

            typeSensors.ObserveOnDispatcher().Subscribe(listSource);
  
        }

        private void QueryAny_Click(object sender, RoutedEventArgs e)
        {
            var anySensor = sensor.Any(v => v.SensorValue > 17);
            anySensor.ObserveOnDispatcher().Subscribe(v => MessageBox.Show(v.ToString()));
        }

        private void Heartbeat_Click(object sender, RoutedEventArgs e)
        {
            var heartbeat = sensor.BufferWithTime(TimeSpan.FromSeconds(3));

            heartbeat.ObserveOnDispatcher().Subscribe(v => Heartbeat.Content = v.
                Where(val => val.SensorType == "2").Count());

        }
    }
}
