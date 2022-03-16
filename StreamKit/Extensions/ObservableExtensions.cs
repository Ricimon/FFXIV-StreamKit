using System;
using System.Reactive.Disposables;

namespace StreamKit.Extensions
{
    public static class DisposableExtensions
    {
        public static void DisposeWith(this IDisposable disposable, CompositeDisposable disposables)
            => disposables.Add(disposable);
    }
}
