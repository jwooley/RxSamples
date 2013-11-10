using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] items = {"1", "2", "3"};
            string key = "2";
            bool success = false;
            if (items.Any(item => item == key))
                success = true;
            Console.WriteLine(success);
            Console.ReadLine();

            //const string INPUT = "reactive";
            //var rand = new Random();
            //var input = Observable.GenerateWithTime(
            //    3,
            //    len => len <= INPUT.Length,
            //    len => len + 1,
            //    len => INPUT.Substring(0, len),
            //    _ => TimeSpan.FromMilliseconds(rand.Next(200, 1200))
            //        );
        }
    }
}
