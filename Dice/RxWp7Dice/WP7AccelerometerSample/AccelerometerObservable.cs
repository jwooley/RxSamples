using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Reactive;
using Microsoft.Xna.Framework;
using Microsoft.Devices.Sensors;

namespace WP7AccelerometerSample
{
    public class AccelerometerObservable
    {
        public static IObservable<Vector3> GetAccelerometer()
        {
            var accelerometer = new Accelerometer();
            accelerometer.Start();

            IObservable<IEvent<AccelerometerReadingEventArgs>> accelerometerReadingAsObservable =
                 Observable.FromEvent<AccelerometerReadingEventArgs>(
                     ev => accelerometer.ReadingChanged += ev,
                     ev => accelerometer.ReadingChanged -= ev);

            var vector3FromAccelerometerEventArgs = from args in accelerometerReadingAsObservable
                                                    select new Vector3(
                                                        (float)args.EventArgs.X,
                                                        (float)args.EventArgs.Y,
                                                        (float)args.EventArgs.Z);
            return vector3FromAccelerometerEventArgs;
        }
    }
}
