using System;

using System.Threading;
using System.Threading.Tasks;

namespace Dice
{
    public static class Randomizer
    {
        private static Lazy<Random> rand = new Lazy<Random>(() => new Random(DateTime.Now.Millisecond));

        public static int Roll(int sideCount)
        {

            int newValue = (int)(rand.Value.NextDouble() * sideCount) + 1;

            System.Threading.Thread.Sleep(newValue * 200);
            return newValue;
        }
        public static async Task<int> RollAsync(int sideCount)
        {
            int newVal = (int)(rand.Value.NextDouble() * sideCount) + 1;
            // Simulate a long running operation
            await Task.Delay(newVal * 200);
            return newVal;
        }

    }

    public class AsyncSimulator
    {
        static System.Threading.AutoResetEvent autoEvent = new AutoResetEvent(false);

        public static void DoWork()
        {
            ThreadPool.QueueUserWorkItem(WorkMethod, autoEvent);
            autoEvent.WaitOne();
        }
        static void WorkMethod(Object stateInfo)
        {
            Thread.Sleep(new Random().Next(100, 2000));
            ((AutoResetEvent)stateInfo).Set();
        }
    }
}
