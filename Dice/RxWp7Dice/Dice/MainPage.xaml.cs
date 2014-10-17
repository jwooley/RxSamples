using System;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Reactive;
using Microsoft.Devices.Sensors;
using Gestures;

namespace Dice
{
    public partial class MainPage : PhoneApplicationPage
    {
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            accel = new Accelerometer();
            accel.Start();

            SetupObservableFinished();
        }
        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            _diceObserver.Dispose();
            _diceObserver = null;

            accel.Stop();
            accel.Dispose();
        }

        // Constructor
        public MainPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            //this.RollButton.Click += RollClick;
        }

        Accelerometer accel = new Accelerometer();
        private IDisposable _diceObserver;


        //private void RollClick(object sender, RoutedEventArgs e)
        private void SetupObservable()
        {
            var rollRequested = Observable.FromEvent<RoutedEventArgs>(RollButton, "Click");

            var rollObserver =  from dietoRoll in 
                (from roll in rollRequested
                from die in App.ViewModel.Items.ToObservable()
                where !die.Frozen
                select die)
                .Do(die => die.DotCount = null)
                .SubscribeOn(Scheduler.ThreadPool)
                select new { dietoRoll, rolledDots = Randomizer.Roll(App.ViewModel.SideCount) };

          _diceObserver =  rollObserver.ObserveOnDispatcher().Subscribe(result => result.dietoRoll.DotCount = result.rolledDots);
        }

        private void RollClickStart(object sender, RoutedEventArgs e)
        //private void SetupObservable()
        {
            var roll = from die in App.ViewModel.Items
                       where !die.Frozen
                       select new { die, rolledDots = Randomizer.Roll(App.ViewModel.SideCount) };

            foreach (var result in roll)
            {
                result.die.DotCount = result.rolledDots;
            }
        }

        private void SetupObservableService()
        {
            var buttonObservable = Observable.FromEvent<RoutedEventArgs>(RollButton, "Click");
            RandomizerService.IRandomizer proxy = new RandomizerService.RandomizerClient();
            var serviceObservable = Observable.FromAsyncPattern<int, int>(proxy.BeginRoll, proxy.EndRoll);

            var rollRequested = (from buttonClick in buttonObservable
                                 select new Unit())
                            .Merge(ShakeObserver.GetShakeObserver(accel))
                            .Merge(App.ViewModel.DiceCountDataSource.Select(_ => new Unit()));


            var query = from roll in rollRequested.Throttle(TimeSpan.FromMilliseconds(400))
                        from dice in App.ViewModel.Items.ToObservable()
                           .Where(die => !die.Frozen)
                           .ObserveOnDispatcher()
                           .Do(die => die.DotCount = null)
                           .Zip(Observable.Interval(TimeSpan.FromMilliseconds(250)),
                               (die, _) => die)
                        from serviceResult in serviceObservable.Invoke(App.ViewModel.SideCount)
                        .TakeUntil(rollRequested)
                        select new { source = dice, result = serviceResult };

            _diceObserver = query.ObserveOnDispatcher().Subscribe(val => val.source.DotCount = val.result,
                er => this.ErrorBlock.Text = er.Message);
        }

        private void SetupObservableFinished()
        {

            RandomizerService.IRandomizer proxy = new RandomizerService.RandomizerClient();
            var serviceObservable = Observable.FromAsyncPattern<int, int>(proxy.BeginRoll, proxy.EndRoll);

            var RollObserved = Observable.FromEvent<RoutedEventArgs>(RollButton, "Click").Select(_ => new Unit())
                //.Merge(App.ViewModel.SideCountDataSource.Select(_ => new Unit()))
                .Merge(ShakeObserver.GetShakeObserver(accel))
                .Throttle(TimeSpan.FromMilliseconds(200)).ObserveOnDispatcher();

            var roll = from unfrozenDie in
                           (from click in RollObserved
                            from die in App.ViewModel.Items.ToObservable()
                            where !die.Frozen
                            select die)
                               .Do(die => die.DotCount = null)
                               .Zip(Observable.Interval(TimeSpan.FromMilliseconds(250)), (die, _) => die)
                       //.ObserveOn(Scheduler.ThreadPool)
                       from result in serviceObservable(App.ViewModel.SideCount)
                       .TakeUntil(RollObserved)
                       select new { unfrozenDie, rollValue = result };

            _diceObserver = roll.ObserveOnDispatcher().Subscribe(
                result => result.unfrozenDie.DotCount = result.rollValue,
                err => this.ErrorBlock.Text = err.Message );
        }


    }
}