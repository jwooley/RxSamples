using RxDice.Accelerometers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RxDice
{
    public partial class MainPage : ContentPage
    {
        IDisposable _diceObserver;
        public MainPage()
        {
            InitializeComponent();
            Game.SideCount = 6;
            Game.DiceCount = 5;
            SetupObservable();
            BindingContext = Game;
        }
        public GameModel Game { get; set; } = new GameModel();

        public void SetupObservable()
        {
            

            var RollObserved = Observable.FromEventPattern<EventArgs>(btnRoll, "Clicked").Select(_ => new Unit())
                .Merge(Observable.FromEventPattern<PropertyChangedEventArgs>(Game, "PropertyChanged")
                    .Where(evt => evt.EventArgs.PropertyName == nameof(Game.DiceCount) || evt.EventArgs.PropertyName == nameof(Game.SideCount))
                    .Select(_ => new Unit()))
                .Merge(ShakeObserbable.GetShakeDetections())
                .Throttle(TimeSpan.FromSeconds(.5))
                .ObserveOn(SynchronizationContext.Current);

            var rand = new Random();
            var roll = from unfrozenDie in
                           (from click in RollObserved
                            from die in Game.Dice.ToObservable()
                            where !die.Frozen
                            select die)
                       .Do(die => die.DotCount = null)
                       select new { unfrozenDie, rollValue = rand.Next(Game.SideCount) + 1 };

            _diceObserver = roll.Subscribe(
                result => result.unfrozenDie.DotCount = result.rollValue);
        }
    }
}
