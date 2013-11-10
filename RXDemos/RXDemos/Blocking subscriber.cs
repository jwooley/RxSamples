using System;
using System.Linq;
using System.Threading;

namespace RxDemo
{

    class Program
    {
        static void Main(string[] args)
        {

            // what happens if one subsciber blocks ?
            var obs = Observable.Range(0, 3);

                obs.Subscribe(ii =>
                {
                    if (ii == 0)
                    {
                        Thread.Sleep(1000 * 1000);
                    }
                    Console.WriteLine(ii);
                })
                ;

                obs.Subscribe(ii => Console.WriteLine("s 2 = {0}", ii));
            Console.ReadKey();
        }
    }
}
