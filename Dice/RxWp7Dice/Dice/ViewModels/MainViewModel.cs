using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Phone.Reactive;


namespace Dice
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.SideCountDataSource = new SideCountLoopingSelector { SelectedItem = 6 };
            SideCountDataSource.Subscribe(val => SideCount = val);

            this.DiceCountDataSource = new IntegerLoopingDataSource { MinValue = 1, MaxValue = 9, Increment = 1, SelectedItem = 5 };
            DiceCountDataSource.Subscribe(val => DiceCount = val);

            this.Items = new ObservableCollection<Die>();
            LoadData();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<Die> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Items.Add(new Die() { Index = 1, DotCount = 3, Frozen = false });
            this.Items.Add(new Die() { Index = 2, DotCount = 2, Frozen = true });
            this.Items.Add(new Die() { Index = 3, DotCount = 1, Frozen = false });
            this.Items.Add(new Die() { Index = 4, DotCount = 4, Frozen = true });
            this.Items.Add(new Die() { Index = 5, DotCount = 5, Frozen = false });
            
            this.IsDataLoaded = true;
        }

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
                        Items.Add(new Die { Index = Items.Max(dice => dice.Index) + 1 });
                    NotifyPropertyChanged("DiceCount");
                }
            }
        }

        public SideCountLoopingSelector SideCountDataSource {get; private set;}
        public IntegerLoopingDataSource DiceCountDataSource { get; private set; }

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
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}