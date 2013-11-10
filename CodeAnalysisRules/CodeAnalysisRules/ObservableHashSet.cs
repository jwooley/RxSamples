using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeAnalysisRules
{
    class ObservableHashSet<T> : HashSet<T>, IObservable<T>
    {
        #region IObservable<T> Members

        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
