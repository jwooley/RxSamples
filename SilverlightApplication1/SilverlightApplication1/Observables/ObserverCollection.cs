using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SilverlightApplication1
{
    public class ObserverCollection<T> : ObservableCollection<T>, IObserver<T>
    {

        #region IObserver<T> Members

        public void OnCompleted()
        {
         //   throw new NotImplementedException();
        }

        public void OnError(Exception exception)
        {
           // throw new NotImplementedException();
        }

        public void OnNext(T value)
        {
            this.Add(value);
        }

        #endregion
    }
}
