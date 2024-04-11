using System;

namespace Unity1week202403.Extensions
{
    public static class DisposableExtensions
    {
        public static T AddTo<T>(this T disposable, Presenter controller) where T : IDisposable
        {
            controller.AddDisposable(disposable);
            return disposable;
        }
    }
}