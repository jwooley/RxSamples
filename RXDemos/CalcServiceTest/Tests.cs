using System;
using System.Threading;
using System.Linq;
using RxDemo.WCFServiceRef;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace RxDemo
{
    [TestClass]
    public class Tests:IDisposable
    {
        private IWCFService svc;

        public Tests()
        {
            svc = new WCFServiceClient();
        }

        [TestMethod]
        public void TestWithExplicitBlocking()
        {
            var iar = svc.BeginGetData(0, (cb) => { }, null);
            var actual = svc.EndGetData(iar);
            Assert.AreEqual("You entered: 0", actual);
        }

        [TestMethod]
        public void TestWithARE()
        {
            string actual = null;
            using (AutoResetEvent are = new AutoResetEvent(false))
            {
                svc.BeginGetData(0, cb =>
                {
                    actual = svc.EndGetData(cb);
                    are.Set();
                }, null);
                are.WaitOne();
            }
            Assert.AreEqual("You entered: 0", actual);
        }

        [TestMethod]
        public void TestWithRx()
        {
            Func<int, IObservable<string>> svcrx = Observable.FromAsyncPattern<int, string>(svc.BeginGetData, svc.EndGetData);
            string actual = svcrx(0).Single();
            Assert.AreEqual("You entered: 0", actual);
        }

        [TestMethod]
        public void TestWithRx2()
        {
            string actual = svc.GetDataAsync(0).Single();
            Assert.AreEqual("You entered: 0", actual);
        }

        public void Dispose()
        {
            ((IDisposable)svc).Dispose();
        }
    }
}
