using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RxDice
{
    public class DiceModel : INotifyPropertyChanged
    {
        private bool frozen = false;
        private int sideCount;
        private int? dotCount;

        public bool Frozen
        {
            get => frozen; 
            set
            {
                frozen = value;
                NotifyPropertyChanged();
            }
        }

        public int? DotCount
        {
            get => dotCount; 
            set
            {
                dotCount = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
