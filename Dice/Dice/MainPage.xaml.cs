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

            SetupObservable();
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
            RandomizerService.IRandomizer proxy = new RandomizerService.RandomizerClient();
            var svcObservable = Observable.FromAsyncPattern<int, int>(proxy.BeginRoll, proxy.EndRoll);

            var rollDetected = Observable.FromEvent<RoutedEventArgs>(RollButton, "Click").Select(_=> new Unit())
                .Merge(App.ViewModel.DiceCountDataSource.Select(_ => new Unit()))
                .Merge(ShakeObserver.GetShakeObserver(accel));

            var roll =
                from detected in rollDetected.Throttle(TimeSpan.FromMilliseconds(400))
                from die in App.ViewModel.Items.ToObservable()
                .Where(die => !die.Frozen)
                .ObserveOnDispatcher()
                .Do(die => die.DotCount = null)
                from result in Randomizer.RollAsync(App.ViewModel.SideCount).ToObservable()
                select new { die, rolledDots = result };

            _diceObserver = roll.ObserveOnDispatcher().Subscribe(
                result => result.die.DotCount = result.rolledDots,
                err => this.ErrorBlock.Text = err.Message);

        }

        #region Demo Code Archives
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

        private void SetupObservableFinished()
        {
            RandomizerService.IRandomizer svc = new RandomizerService.RandomizerClient();
            var svcObserver = Observable.FromAsyncPattern<int, int>(svc.BeginRoll, svc.EndRoll);

            var rollDetected = Observable.FromEvent<RoutedEventArgs>(RollButton, "Click")
                .Select(_ => new Unit())
                .Merge(ShakeObserver.GetShakeObserver(accel))
                .Merge(App.ViewModel.SideCountDataSource.Select(_ => new Unit()))
                .Throttle(TimeSpan.FromMilliseconds(250))
                ;

            var query =
                from roll in rollDetected
                from die in App.ViewModel.Items.ToObservable()
                .Where(die => !die.Frozen)
                .ObserveOnDispatcher()
                .Do(die => die.DotCount = null)
                //.ObserveOn(Scheduler.ThreadPool)
                from result in svcObserver(App.ViewModel.DiceCount)
                .TakeUntil(rollDetected)
                select new { die, rolledDots = result };

            _diceObserver = query.ObserveOnDispatcher()
                .Subscribe(result => result.die.DotCount = result.rolledDots,
                    err => ErrorBlock.Text = err.Message);
        }
        #endregion

    }
}