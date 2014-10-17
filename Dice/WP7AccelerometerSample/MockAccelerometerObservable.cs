using System;
using System.Threading;
using Microsoft.Phone.Reactive;
using Microsoft.Xna.Framework;

namespace WP7AccelerometerSample
{
    public class MockAccelerometerObservable
    {
        public static IObservable<Vector3> GetAccelerometer()
        {
            var obs = Observable.GenerateWithTime<double, Vector3>(
                0, 
                _ => true,
                theta => new Vector3((float)Math.Sin(theta), (float)Math.Cos(theta * 1.1), (float)Math.Sin(theta * .7)),
                _ => TimeSpan.FromMilliseconds(100),
                theta => theta + .1)
                .ObserveOnDispatcher();

            return obs;
        }
    }
}
