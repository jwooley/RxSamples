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

using System.Linq;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Reactive;

namespace Dice
{
    public class ShakeObserver
    {
        const double MinimumOffset = 1.44;
        const double TimeThreshold = 200;
        public static IObservable<IEvent<AccelerometerReadingEventArgs>> 
            GetObserver(Accelerometer accel)
        {
            var readingChangedObservable =
                Observable.FromEvent<AccelerometerReadingEventArgs>
                (accel, "ReadingChanged");

            var query =  
                from interval in 
                  (from startEvent in readingChangedObservable
                   where (startEvent.EventArgs.X * startEvent.EventArgs.X + 
                        startEvent.EventArgs.Y * startEvent.EventArgs.Y) >
                        MinimumOffset
                   select startEvent).TimeInterval()
                where interval.Interval.TotalMilliseconds < TimeThreshold 
                select interval.Value;

            return query;
        }
    }
}
