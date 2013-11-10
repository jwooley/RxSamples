using System;
using System.Threading;

namespace RxDemo
{
    
    public class WCFService : IWCFService
    {
        public string GetData(int value)
        {
            Thread.Sleep(value);
            return string.Format("You entered: {0}", value);
        }

        public string GetDataSlowly(int value)
        {
            Thread.Sleep(10 * 1000);
            return String.Format("GetDataSlowly {0}", value);
        }
    }
}
