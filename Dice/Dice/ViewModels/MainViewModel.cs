using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;


namespace Dice
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<Die>();
            SideCount = 6;
            DiceCount = 5;
        }

        public ObservableCollection<Die> Items { get; private set; }
        
        public int DiceCount
        {
            get {return Items.Count;}
            set
            {
                while (Items.Count != value)
                {
                    if (Items.Count > value)
                        Items.RemoveAt(0);
                    else
                    {
                        Die newDie = new Die();
                        if (Items.Any())
                            newDie.Index = Items.Max(dice => dice.Index) + 1;
                        else
                            newDie.Index = 1;

                        Randomizer.RollDice(newDie, SideCount);
                        Items.Add(newDie);
                    }
                    NotifyPropertyChanged("DiceCount");
                }
            }
        }

        private int _sideCount = 6;
        public int SideCount
        {
            get { return _sideCount;}
            set 
            {
                if (_sideCount != value)
                {
                    _sideCount = value;
                    NotifyPropertyChanged("SideCount");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}