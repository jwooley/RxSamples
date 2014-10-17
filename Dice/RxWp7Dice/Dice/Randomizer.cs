using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Phone.Reactive;

namespace Dice
{
    public static class Randomizer
    {
        public static void RollDice(Die instance, int sideCount)
        {
            int newValue = Roll(sideCount);
            
            instance.DotCount = newValue;
        }

        public static int Roll(int sideCount)
        {
            var rand = new Random();
            int newValue = (int)(rand.NextDouble() * sideCount) + 1;
            System.Threading.Thread.Sleep(newValue * 100);
            return newValue;
        }

    }
}

namespace Dice.RandomizerService
{
    public partial class RandomizerClient
    {
        public Func<int, int, IObservable<object[]>> RandomizeDiceObservable()
        {
            var obs = Observable.FromAsyncPattern<int, int, object[]>(this.OnBeginRandomDiceResult, this.OnEndRandomDiceResult);
            return obs;
        }

        private IAsyncResult OnBeginRandomDiceResult(int sideCount, int index, AsyncCallback callback, object asyncState)
        {
            object[] parameters = { sideCount, index };
            return OnBeginRandomDiceResult(parameters, callback, asyncState);
        }
        
    }
}