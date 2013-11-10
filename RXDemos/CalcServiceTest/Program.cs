using System;
using System.Linq;

namespace RxDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Observable.Range(0, 5).Scan(0, (count, _) => count + 1).Subscribe(Console.WriteLine);
            Console.ReadKey();
        }
    }
}
