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
using System.Collections.ObjectModel;

namespace SilverlightApplication1.Observables
{
    public class ObserverCollection<T> : ObservableCollection<T>, IObserver<T>
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
            this.Add(value);
        }

        #endregion
    }
}
