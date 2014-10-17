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
using Microsoft.Phone.Controls.Primitives;
using System.ComponentModel;
using Microsoft.Phone.Reactive;
using System.Collections.Generic;

namespace Dice
{
    public class IntegerLoopingDataSource : ILoopingSelectorDataSource, INotifyPropertyChanged, IObservable<int>
    {

        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int Increment { get; set; }

        #region ILoopingSelectorDataSource Members

        public object GetNext(object relativeTo)
        {
            int next = (int)relativeTo + Increment;

            if (next <= MaxValue)
                return next;
            else
                return MinValue;
        }

        public object GetPrevious(object relativeTo)
        {
            int previous = (int)relativeTo - Increment;
            if (previous < MinValue)
                return MaxValue;
            else
                return previous;
        }

        int SelectedValue;
        public object SelectedItem
        {
            get
            {
                return SelectedValue;
            }
            set
            {
                int newVal = (int)value;
                if (SelectedValue != newVal)
                {
                    SelectedValue = newVal;
                    NotifyChange("SelectedValue");
                }
            }
        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        #endregion

        #region INotifyPropertyChanged Members
        private void NotifyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            if (SelectionChanged != null)
                SelectionChanged(this, new SelectionChangedEventArgs(new List<int>() { }, new List<int>() { }));

            ObserverListeners.OnNext(SelectedValue);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IObservable<int> Members

        Subject<int> ObserverListeners = new Subject<int>();
        public IDisposable Subscribe(IObserver<int> observer)
        {
            return ObserverListeners.Subscribe(observer);
        }

        #endregion
    }
}
