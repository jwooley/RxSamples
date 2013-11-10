using Microsoft.Phone.Reactive;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WP7AccelerometerSample.Accelerometers
{
    class MockAccelerometer
    {
        public static IObservable<Vector3> GetAccelerometer()
        {
            var obs = Observable.GenerateWithTime<double, Vector3>(
                    initialState: 0,
                    condition: _ => true,
                    iterate: theta => theta + .1,
                    resultSelector: theta => new Vector3((float)Math.Sin(theta), (float)Math.Cos(theta + 1.1), (float)Math.Sin(theta + .7)),
                    timeSelector: _ => TimeSpan.FromMilliseconds(100)
                )
                .ObserveOnDispatcher();

            return obs;
        }
    }
}
