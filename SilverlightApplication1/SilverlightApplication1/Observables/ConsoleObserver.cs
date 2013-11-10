using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SilverlightApplication1
{
    public class ConsoleObserver<T> : IObserver<T>
    {
        #region IObserver<T> Members

        public void OnCompleted()
        {
//            throw new NotImplementedException();
        }

        public void OnError(Exception exception)
        {
//            throw new NotImplementedException();
        }

        public void OnNext(T value)
        {
            Debug.WriteLine(value.ToString());
        }

        #endregion
    }
}
