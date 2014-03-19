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
using System.Reactive.Linq;

namespace SilverlightApplication1
{

    public class ObservableSensor: IObservable<SensorInfo>
    {
        private IObservable<SensorInfo> source;
        private bool _running;

        public IDisposable Subscribe(IObserver<SensorInfo> observer)
        {
            _running = true;
            var handle = source.Subscribe(observer);
            return handle;
        }

        public void StartSensor()
        {
            var randomizer = new Random(DateTime.Now.Millisecond);
            source = Observable.Generate(
                initialState: 0.0,
                condition: _ => _running,
                iterate: _ => randomizer.NextDouble(),
                resultSelector: x => new SensorInfo
                    {
                        SensorType = ((int)(x * 4)).ToString(),
                        SensorValue = x * 20,
                        TimeStamp = DateTime.Now
                    },
                timeSelector: x => TimeSpan.FromMilliseconds(x * 100)
            );
        }

        public void StopSensor()
        {
            _running = false;
        }
    }
}
