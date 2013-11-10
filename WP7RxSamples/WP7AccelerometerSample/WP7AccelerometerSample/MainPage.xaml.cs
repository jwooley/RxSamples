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

        IDisposable subscription;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (subscription == null)
            {
                var accelObs = Accelerometers.AccelerometerFactory.GetAccelerometer();
                subscription = accelObs.ObserveOnDispatcher().Subscribe(data =>
                    {
                        ReadingTextBlock.Text =
                           " x: " + data.X.ToString("0.00") +
                           " y: " + data.Y.ToString("0.00") +
                           " z: " + data.Z.ToString("0.00");

                        Single multiplier;
                        if (Single.TryParse(this.MoveMultiplier.Text, out multiplier))
                        {
                            ButtonTransform.X = data.X * multiplier;
                            ButtonTransform.Y = data.Y * multiplier;
                        }
                    });
            }
            else
            {
                subscription.Dispose();
                subscription = null;
            }
        }
    }
}
