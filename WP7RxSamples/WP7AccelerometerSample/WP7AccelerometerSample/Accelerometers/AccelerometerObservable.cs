using Microsoft.Devices.Sensors;
using Microsoft.Phone.Reactive;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WP7AccelerometerSample.Accelerometers
{
    class AccelerometerObservable
    {
        public static IObservable<Vector3> GetObservable()
        {
            var accelerometer = new Accelerometer();
            
            var obs = Observable.FromEvent<SensorReadingEventArgs<AccelerometerReading>>(
                ev => accelerometer.CurrentValueChanged += ev,
                ev => accelerometer.CurrentValueChanged -= ev);
            
            accelerometer.Start();

            var obsVector = from args in obs
                            select new Vector3((float)args.EventArgs.SensorReading.Acceleration.X,
                                (float)args.EventArgs.SensorReading.Acceleration.Y,
                                (float)args.EventArgs.SensorReading.Acceleration.Z);
            
            return obsVector;

        }
    }
}
