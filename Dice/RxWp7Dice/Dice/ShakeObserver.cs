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
using System.Collections;
using System.Collections.Generic;

namespace Dice
{
    public class ShakeObserver
    {
        const double MinimumOffset = 1.44;
        const double TimeThreshold = 200;
        public static IObservable<IEvent<AccelerometerReadingEventArgs>> GetShakeObserver(Accelerometer accel)
        {
            var query =  
                from interval in 
                (from startEvent in Observable.FromEvent<AccelerometerReadingEventArgs>(accel, "ReadingChanged")
                   where startEvent.EventArgs.X*startEvent.EventArgs.X + 
                        startEvent.EventArgs.Y*startEvent.EventArgs.Y > MinimumOffset
                   select startEvent).TimeInterval()
                   where interval.Interval.TotalMilliseconds < TimeThreshold 
                   select interval.Value;

            return query;
        }
    }

    public class foo<T> : IEnumerator<T>, IObserver<T>
    {
        public T Current
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        object IEnumerator.Current
        {
            get { throw new NotImplementedException(); }
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void OnNext(T value)
        {
            throw new NotImplementedException();
        }
    }

}
