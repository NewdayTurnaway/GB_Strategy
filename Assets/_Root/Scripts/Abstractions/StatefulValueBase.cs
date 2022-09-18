using System;
using UniRx;

namespace Abstractions
{
    public abstract class StatefulValueBase<T> : ValueBase<T>, IObservable<T>
    {
        private readonly ReactiveProperty<T> _reactiveProperty = new();

        public override void SetValue(T value)
        {
            base.SetValue(value);
            _reactiveProperty.Value = value;
        }

        public IDisposable Subscribe(IObserver<T> observer) => 
            _reactiveProperty.Subscribe(observer);
    }
}