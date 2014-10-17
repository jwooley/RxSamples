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
    public class SideCountLoopingSelector : ILoopingSelectorDataSource, INotifyPropertyChanged, IObservable<int>
    {

        #region ILoopingSelectorDataSource Members

        public SideCountLoopingSelector()
        {
            SideCountValues = new int[] { 3, 4, 6, 8, 10, 12, 20 };
            SideCount = SideCountValues.Length;
        }

        int[] SideCountValues;
        int SelectedValue = 0;
        int SideCount;

        public int IndexOf(int value)
        {
            for (int index = 0; index < SideCount; index++)
            {
                if (SideCountValues[index] == value)
                    return index;
            }
            return -1;
        }

        public object GetNext(object relativeTo)
        {
            int current = (int)relativeTo;
            int currentIndex = IndexOf(current);
            int nextIndex = -1;
            if (currentIndex < SideCount -1)
                nextIndex = currentIndex + 1;
            else
                nextIndex = 0;

            // Not found, return first value
            if (nextIndex == -1)
                nextIndex = 0;

            return SideCountValues[nextIndex];
        }

        public object GetPrevious(object relativeTo)
        {
            int current = (int)relativeTo;
            int currentIndex = IndexOf(current);
            int nextIndex = -1;
            if (currentIndex == 0)
                nextIndex = SideCount - 1;
            else
                nextIndex = currentIndex - 1;

            // Not found, return first value
            if (nextIndex == -1)
                nextIndex = 0;

            return SideCountValues[nextIndex];
        }

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
