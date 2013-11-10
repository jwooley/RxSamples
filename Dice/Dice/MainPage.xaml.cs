using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Reactive;
using Microsoft.Devices.Sensors;
using Dice.RandomizerService;

namespace Dice
{
    public partial class MainPage : PhoneApplicationPage
    {
        private IDisposable _diceObserver;

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            SetupObservable2();
        }
        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (_diceObserver != null)
            {
                _diceObserver.Dispose();
                _diceObserver = null;
            }
        }

        private void SetupObservable()
        {
            var buttonObservable = Observable.FromEvent<RoutedEventArgs>(RollButton, "Click");

            var query = from buttonClick in buttonObservable
                        from dice in App.ViewModel.Items.ToObservable()
                        where !dice.Frozen
                        select dice;

            _diceObserver = query.Subscribe(dice =>
                Randomizer.RollDice(dice, App.ViewModel.SideCount));
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            //this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            //this.RollButton.Click += new RoutedEventHandler(RollButton_ClickObservable);
            accel = new Accelerometer();
            accel.Start();
        }

        void RollButton_Click(object sender, RoutedEventArgs e)
        {
            var unfrozenDice = from dice in App.ViewModel.Items
                               where !dice.Frozen
                               select dice;

            foreach (var dice in unfrozenDice)
            {
                Randomizer.RollDice(dice, App.ViewModel.SideCount);
            }
        }

        void RollButton_ClickObservable(object sender, RoutedEventArgs e)
        {
            var unfrozenDice = from dice in App.ViewModel.Items
                                   .ToObservable()
                               where !dice.Frozen
                               select dice;

            unfrozenDice.Subscribe( dice =>
                Randomizer.RollDice(dice, App.ViewModel.SideCount));
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        Accelerometer accel = new Accelerometer();



        private void SetupObservable2()
        {

            var buttonObservable = 
                Observable.FromEvent<RoutedEventArgs>
                    (RollButton, "Click");

            var rollDetected = 
                (from buttonClick in buttonObservable
                 select (EventArgs)buttonClick.EventArgs)
                .Merge(from shaken in ShakeObserver.GetObserver(accel)
                 select (EventArgs)shaken.EventArgs);

            var query = (from evt in rollDetected
                         from dice in App.ViewModel.Items.ToObservable()
                         where !dice.Frozen
                         select dice)
                         .ObserveOnDispatcher();
              
            _diceObserver = query.Subscribe(dice =>
                Randomizer.RollDice(dice, App.ViewModel.SideCount));
        }


        private void SetupObservableService()
        {
            var buttonObservable = Observable.FromEvent<RoutedEventArgs>(RollButton, "Click");
            IRandomizer proxy = new RandomizerService.RandomizerClient() as Dice.RandomizerService.IRandomizer;
            var svcObserver = Observable.FromAsyncPattern<int,int>(proxy.BeginRandomDiceResult, proxy.EndRandomDiceResult);

            var rollDetected =
                (from buttonClick in buttonObservable
                 select (EventArgs)buttonClick.EventArgs)
                .Merge(from shaken in ShakeObserver.GetObserver(accel)
                 select (EventArgs)shaken.EventArgs)
                .Throttle(TimeSpan.FromMilliseconds(500));

            var query =
                (from evt in rollDetected
                 from dice in App.ViewModel.Items.ToObservable()
                 where !dice.Frozen
                 from result in svcObserver.Invoke(App.ViewModel.SideCount)
                 .TakeUntil(rollDetected)
                 select new { dice, svcResult = result })
                .ObserveOnDispatcher();

            _diceObserver = query.Subscribe(val => val.dice.DotCount = val.svcResult);
        }
    }
}