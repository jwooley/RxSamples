using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application1.Accelerometers
{
    public static class FakeAccelerometer
    {
        public static IObservable<Vector3> GetAccelerometer()
        {
            var obs = Observable.Generate<double, Vector3>(
                initialState: 0,
                condition: _ => true,
                iterate: theta => theta + .1,
                resultSelector: theta => new Vector3{X=Math.Sin(theta), Y=Math.Cos(theta * 1.1), Z=Math.Sin(theta * .7)},
                timeSelector: _ => TimeSpan.FromMilliseconds(100)
                )
                .ObserveOnDispatcher();

            return obs;
        }
    }
}
