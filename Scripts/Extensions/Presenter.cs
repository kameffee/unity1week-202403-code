using System;
using R3;

namespace Unity1week202403.Extensions
{
    public class Presenter : IDisposable
    {
        private readonly CompositeDisposable _disposable = new();

        public void AddDisposable(IDisposable item)
        {
            _disposable.Add(item);
        }

        void IDisposable.Dispose() => _disposable.Dispose();
    }
}