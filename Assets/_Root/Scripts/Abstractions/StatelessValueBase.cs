using System;
using UniRx;

namespace Abstractions
{
    public abstract class StatelessValueBase<T> : ValueBase<T>, IObservable<T>
    {
        private readonly Subject<T> _subject = new();

        public override void SetValue(T value)
        {
            base.SetValue(value);
            _subject.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<T> observer) => 
            _subject.Subscribe(observer);
    }
}