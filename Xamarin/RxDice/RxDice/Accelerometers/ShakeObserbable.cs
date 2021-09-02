using System;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Reactive;
using Xamarin.Essentials;

namespace RxDice.Accelerometers
{
    public class ShakeObserbable
    {
        const double MinimumOffset = 1.44;
        const double TimeThreshold = 200;
        public static IObservable<Unit> GetShakeObserver()
        {
            var readings = AccelerometerObservable.GetAccelerometer();
            var query =
                readings.Where(startEvent => startEvent.X * startEvent.X +
                      startEvent.Y * startEvent.Y > MinimumOffset)
                .TimeInterval().Where(interval => interval.Interval.TotalMilliseconds < TimeThreshold)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Select(_ => new Unit());

            return query;
        }

        public static IObservable<Unit> GetShakeDetections()
        {
            Accelerometer.Start(SensorSpeed.Default);
            return Observable.FromEventPattern(
                h => Accelerometer.ShakeDetected += h,
                h => Accelerometer.ShakeDetected -= h)
                .Select(_ => new Unit());
        }
    }
}
