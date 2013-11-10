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
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Phone.Reactive;

namespace WP7AccelerometerSample
{
    public class AccelerometerEmulator: IObservable<Vector3>, IDisposable
    {

        #region IObservable<Vector3> Members

        IObserver<Vector3> accelObserver;

       

        public IDisposable Subscribe(IObserver<Vector3> observer)
        {
            accelObserver = observer;

            Random rnd = new Random();
            for (double theta = 0; accelObserver != null ; theta += .1)
            {
                Vector3 reading = new Vector3(
                    (float)Math.Sin(theta),
                    (float)Math.Cos(theta * 1.1),
                    (float)Math.Sin(theta * .7));
                reading.Normalize();

                if (rnd.NextDouble() > .95)
                {
                    reading = new Vector3(
                        (float)(rnd.NextDouble() * 3.0 - 1.5),
                        (float)(rnd.NextDouble() * 3.0 - 1.5),
                        (float)(rnd.NextDouble() * 3.0 - 1.5));
                }
                // return the vector and sleep before repeating
                observer.OnNext(reading);
                Thread.Sleep(100);
            }
            return this;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            accelObserver = null;
        }

        #endregion
    }
}
