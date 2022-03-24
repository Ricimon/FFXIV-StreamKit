using NUnit.Framework;
using System;
using System.Reactive.Linq;

namespace StreamKitTests.UnitTests
{
    public class ObservableTests
    {
        private delegate void CustomDelegate(bool arg1, int arg2);

        private event CustomDelegate CustomEvent;

        [Test]
        public void SubscribingToCustomEventDelegate_ShouldWork()
        {
            Observable.FromEvent<CustomDelegate, (bool, int)>(
                h => (bool b, int i) => h((b, i)),
                h => this.CustomEvent += h,
                h => this.CustomEvent -= h)
                .Subscribe(args =>
                {
                    var conditionFlag = args.Item1;
                    var value = args.Item2;
                })
                .Dispose();
        }
    }
}
