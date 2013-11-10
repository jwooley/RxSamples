using System;
using System.Collections.Generic;
using System.Linq;

namespace RxDemo
{
    class HotAndColdDemo
    {
        static void Main(string[] args)
        {

            Random rnd = new Random();


            IEnumerable<int> enColdDeterministic = Enumerable.Range(0, 5);
            IEnumerable<string> zipCold = enColdDeterministic.Zip(enColdDeterministic, (left, right) => string.Format("{0}->{1}", left, right));
            Console.WriteLine("ColdDeterministic enum == "+ String.Join(", ", zipCold.ToArray()));


            IEnumerable<int> enColdNonDet = enColdDeterministic.Select(ii => rnd.Next(9));
            IEnumerable<string> zipColdND = enColdNonDet.Zip(enColdNonDet, (left, right) => string.Format("{0}->{1}", left, right));
            Console.WriteLine("Cold NonDeterministic enum == " + String.Join(", ", zipColdND.ToArray()));
            Console.WriteLine("Huh?");

            int[] enHotNonDet = enColdDeterministic.Select(ii => rnd.Next(9)).ToArray();
            IEnumerable<string> zipHotND = enHotNonDet.Zip(enHotNonDet, (left, right) => string.Format("{0}->{1}", left, right));
            Console.WriteLine("Hot NonDeterministic enum == " + String.Join(", ", zipHotND.ToArray()));

            Console.WriteLine();
            
            
            var randomxs = Observable
                .Generate(0, ii => true, ii => ii, ii => rnd.Next(9))
                .Take(10)
                .Replay(System.Concurrency.Scheduler.CurrentThread)
                ;

            randomxs
                .Zip(randomxs, (left, right) => left - right)
                .Subscribe(Console.WriteLine);

            randomxs.Connect();
            

            Console.WriteLine("press any key");
            Console.ReadKey();
        }
        
    }
}

//
//randomxs.Subscribe(Console.WriteLine);
//randomxs.Subscribe(a=> Console.WriteLine("A: {0}",a));
//randomxs.Subscribe(b => Console.WriteLine("B: {0}", b));
