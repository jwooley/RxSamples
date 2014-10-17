using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Dice
{
    public class Die : INotifyPropertyChanged
    {
        private int _Index;
        public int Index
        {
            get
            {
                return _Index;
            }
            set
            {
                if (value != _Index)
                {
                    _Index = value;
                    NotifyPropertyChanged("Index");
                }
            }
        }

        private int? _DotCount;
        public int? DotCount
        {
            get
            {
                return _DotCount;
            }
            set
            {
                if (!Nullable.Equals(value, _DotCount))
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

        public Grid DiceGrid
        {
            get
            {
                Grid container = new Grid
                {
                    Height = 54,
                    Width = 54
                };
                for (int i = 0; i < 2; i++)
                {
                    container.RowDefinitions.Add(new RowDefinition());
                    container.ColumnDefinitions.Add(new ColumnDefinition());
                }
                switch (DotCount)
                {
                    case 1:
                        {
                            AddDot(container, 1, 1);
                            break;
                        }
                    case 2:
                        {
                            AddDot(container, 0, 2);
                            AddDot(container, 2, 0);
                            break;
                        }
                    case 3:
                        {
                            AddDot(container, 0, 2);
                            AddDot(container, 1, 1);
                            AddDot(container, 2, 0);
                            break;
                        }
                    case 4:
                        {
                            AddDot(container, 0, 0);
                            AddDot(container, 0, 2);
                            AddDot(container, 2, 0);
                            AddDot(container, 2, 2);
                            break;
                        }
                    case 5:
                        {
                            AddDot(container, 0, 0);
                            AddDot(container, 0, 2);
                            AddDot(container, 1, 1);
                            AddDot(container, 2, 0);
                            AddDot(container, 2, 2);
                            break;
                        }
                    case 6:
                        {
                            AddDot(container, 0, 0);
                            AddDot(container, 0, 2);
                            AddDot(container, 1, 0);
                            AddDot(container, 1, 2);
                            AddDot(container, 2, 0);
                            AddDot(container, 2, 2);
                            break;
                        }
                    default:
                        // no dots
                        break;
                }
                return container;
            }
        }
        private void AddDot(Grid diceGrid, int row, int column)
        {
            Ellipse dot = new Ellipse
            {
                Width = 12,
                Height = 12,
                Fill = new SolidColorBrush(Colors.Black)
            };
            dot.SetValue(Grid.ColumnProperty, column);
            dot.SetValue(Grid.RowProperty, row);
            diceGrid.Children.Add(dot);
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