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

            //var obs = Observable.Create<Vector3>(subscriber =>
            //{
            //    Random rnd = new Random();
            //    for (double theta = 0; ; theta += .1)
            //    {
            //        Vector3 reading = new Vector3(
            //            (float)Math.Sin(theta),
            //            (float)Math.Cos(theta * 1.1),
            //            (float)Math.Sin(theta * .7));
            //        reading.Normalize();

            //        if (rnd.NextDouble() > .95)
            //        {
            //            reading = new Vector3(
            //                (float)(rnd.NextDouble() * 3.0 - 1.5),
            //                (float)(rnd.NextDouble() * 3.0 - 1.5),
            //                (float)(rnd.NextDouble() * 3.0 - 1.5));
            //        }
            //        // return the vector and sleep before repeating
            //        Thread.Sleep(100);
            //        subscriber.OnNext(reading);
            //    }
            //});
            return obs;
        }
    }
}
