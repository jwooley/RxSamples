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
using Microsoft.Phone.Controls;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Phone.Reactive;

namespace WP7AccelerometerSample
{
    /// <summary>
    /// Sample adapted from MSDN Documentation at 
    /// http://msdn.microsoft.com/en-us/library/ff637521(v=VS.92).aspx
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {

        IDisposable subscribed;
    
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (subscribed == null)
            {
                IObservable<Vector3> accelReadings;

                if (Microsoft.Devices.Environment.DeviceType == Microsoft.Devices.DeviceType.Device)
                    accelReadings = AccelerometerObservable.GetAccelerometer();
                else
                    accelReadings = MockAccelerometerObservable.GetAccelerometer();


                subscribed = accelReadings.Subscribe(args =>
                        {
                            Single multiplier;
                            if (Single.TryParse(this.MoveMultiplier.Text, out multiplier))
                            {
                                ButtonTransform.X = args.X * multiplier;
                                ButtonTransform.Y = args.Y * multiplier;
                            }
                        });
            }
            else
            {
                subscribed.Dispose();
                subscribed = null;
            }
        }
    }
}
