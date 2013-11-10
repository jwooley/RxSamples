using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WP7AccelerometerSample.Accelerometers
{
    class AccelerometerFactory
    {
        public static IObservable<Vector3> GetAccelerometer()
        {
            if (Microsoft.Devices.Sensors.Accelerometer.IsSupported)
                return AccelerometerObservable.GetObservable();
            else
                return MockAccelerometer.GetAccelerometer();
        }
    }
}
