using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;

namespace Application1.Accelerometers
{
    class AccelerometerObservable
    {
        public static IObservable<Vector3> GetAccelerometer()
        {
            var accelerometer = Accelerometer.GetDefault();

            if (null == accelerometer)
                return FakeAccelerometer.GetAccelerometer();

            var accelerometerReadingAsObservable =
                 Observable.FromEventPattern<AccelerometerReadingChangedEventArgs>(accelerometer, "ReadingChanged");

            var vector3FromAccelerometerEventArgs =
                from args in accelerometerReadingAsObservable
                select new Vector3{
                    X = args.EventArgs.Reading.AccelerationX,
                    Y = args.EventArgs.Reading.AccelerationY,
                    Z = args.EventArgs.Reading.AccelerationZ
                };

            return vector3FromAccelerometerEventArgs.ObserveOnDispatcher();
        }
    }
}
