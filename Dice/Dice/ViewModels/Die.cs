using System;
using System.ComponentModel;

namespace Dice
{
    public class Die : INotifyPropertyChanged
    {
        private int _Index;
        public int Index
        { get { return _Index; }
          set {
                if (value != _Index)
                {
                    _Index = value;
                    NotifyPropertyChanged("Index");
                }
            }
        }

        private int _DotCount;
        public int DotCount
        {
            get
            {
                return _DotCount;
            }
            set
            {
                if (value != _DotCount)
                {
                    _DotCount = value;
                    NotifyPropertyChanged("DotCount");
                }
            }
        }

        private bool _Frozen;
        public bool Frozen
        {
            get
            {
                return _Frozen;
            }
            set
            {
                if (value != _Frozen)
                {
                    _Frozen = value;
                    NotifyPropertyChanged("Frozen");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}