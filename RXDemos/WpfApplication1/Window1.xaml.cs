using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace RxDemo
{

    public partial class Window1 : Window
    {
        #region DP SearchText string
        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText", typeof(string), typeof(Window1), new UIPropertyMetadata(""));

        public string SearchText
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (string)GetValue(SearchTextProperty);
            }
            set
            {
                SetValue(SearchTextProperty, value);
            }
        }
        #endregion
        
        public ObservableCollection<String> InputKeys { get; private set; }
        public ObservableCollection<String> InputKeys2 { get; private set; }
        public Window1()
        {
            InputKeys = new ObservableCollection<String>();
            InputKeys2 = new ObservableCollection<String>();
            DataContext = this;
            InitializeComponent();


            //txt.KeyDown += KeyDownHandler1;
            //txt.KeyDown += KeyDownHandler2;

            IObservable<IEvent<KeyEventArgs>> keypresses = Observable.FromEvent<KeyEventArgs>(txt, "KeyDown");

            IObservable<String> goodKeys = keypresses
                                        .Select(IKP => IKP.EventArgs.Key)
                                        .Where(key => key >= Key.A && key <= Key.Z)
                                        .Select(key => key.ToString())
                                        ;

            IDisposable unsub = goodKeys.Subscribe(key => InputKeys.Insert(0, key));
            IDisposable unsub2 = goodKeys.Subscribe(key => InputKeys2.Insert(0, key));

            btn.Click += (sender, e) => unsub.Dispose();
            //btn.Click += (sender, e) => txt.KeyDown -= KeyDownHandler1;




        }
        private void KeyDownHandler1(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.A && e.Key <= Key.Z)
                InputKeys.Insert(0,e.Key.ToString());
        }

        private void KeyDownHandler2(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.A && e.Key <= Key.Z)
                InputKeys2.Insert(0,e.Key.ToString());
        }

    }
}



//Delayed Expensive Search
            //goodKeys
            //    .Do(_ => SearchText = "")
            //    .Throttle(new TimeSpan(0, 0, 0, 1, 500))
            //    .ObserveOnDispatcher()
            //    .Subscribe(key => SearchText = "Searching...");


