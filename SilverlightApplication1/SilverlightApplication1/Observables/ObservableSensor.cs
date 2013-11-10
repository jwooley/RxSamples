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
using System.Collections.Generic;
using System.Linq;

namespace SilverlightApplication1
{

    public class ObservableSensor: IObservable<SensorInfo>, IDisposable
    {
        List<IObserver<SensorInfo>> _observers = new List<IObserver<SensorInfo>>();
        bool _running;

        public void StartSensor()
        {
            if (!_running)
            {
                _running = true;
                Random randomizer = new Random(DateTime.Now.Millisecond);
                while (_running)
                {
                    double randVal = randomizer.NextDouble();
                    if (_observers.Any())
                    {
                        SensorInfo info = new SensorInfo
                        {
                            SensorType = ((int)(randVal * 4)).ToString(),
                            SensorValue = randVal * 20,
                            TimeStamp = DateTime.Now
                        };
                        _observers.ForEach(o =>  o.OnNext(info));
                    }
                    System.Threading.Thread.Sleep((int)randomizer.NextDouble() * 1000);
                }
            }
        }

        public void StopSensor()
        {
            _running = false;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_observers != null)
            {
                _observers.ForEach(o => o.OnCompleted());
                _observers.Clear();
            }
        }

        #endregion

        #region IObservable<SensorInfo> Members

        public IDisposable Subscribe(IObserver<SensorInfo> observer)
        {
            _observers.Add(observer);
            if (!_running)
                _running = true;
            return this;
        }

        #endregion
    }
}
