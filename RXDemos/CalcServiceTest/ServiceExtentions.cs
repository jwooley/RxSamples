using System;
using System.Linq;
using RxDemo.WCFServiceRef;

namespace RxDemo
{
    public static class ServiceExtentions
    {
        public static IObservable<string> GetDataAsync(this IWCFService svc, int arg)
        {
            return Observable.FromAsyncPattern<int, string>(svc.BeginGetData, svc.EndGetData)(arg);
        }

        public static IObservable<string> GetDataSlowlyAsync(this IWCFService svc, int arg)
        {
            return Observable.FromAsyncPattern<int, string>(svc.BeginGetDataSlowly, svc.EndGetDataSlowly)(arg);
        }

    }
}
