using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RxDice
{
    public class GameModel : INotifyPropertyChanged
    {
        public GameModel()
        {
            Dice.CollectionChanged += (o, e) => NotifyPropertyChanged(nameof(Dice));
        }

        public ObservableCollection<DiceModel> Dice { get; set; } = new ObservableCollection<DiceModel>();
        public int DiceCount
        {
            get { return Dice.Count; }
            set
            {
                while (Dice.Count != value)
                {
                    if (Dice.Count > value)
                        Dice.RemoveAt(0);
                    else
                        Dice.Add(new DiceModel { });
                    NotifyPropertyChanged();
                }
            }
        }

        int _SideCount;
        public int SideCount
        {
            get { return _SideCount; }
            set 
            { 
                _SideCount = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName]string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
