using Android.Hardware;
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reactive.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.Platform.Android;

namespace RxDice.Accelerometers
{
    class AccelerometerObservable
    {
        public static IObservable<Vector3> GetAccelerometer()
        {
            Accelerometer.Start(SensorSpeed.Default);
            var obs = Observable.FromEventPattern<AccelerometerChangedEventArgs>(
                h => Accelerometer.ReadingChanged += h,
                h => Accelerometer.ReadingChanged -= h);

            var obsVector = from args in obs
                            select new Vector3(
                                args.EventArgs.Reading.Acceleration.X,
                                args.EventArgs.Reading.Acceleration.Y,
                                args.EventArgs.Reading.Acceleration.Z);
            return obsVector;
        }
    }
}
