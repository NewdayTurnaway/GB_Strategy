using System;
using Utils;

namespace Abstractions
{
    public abstract class ValueBase<T> : IAwaitable<T>
    {
        public T CurrentValue { get; private set; }
        public event Action<T> OnNewValue;

        public void SetValue(T value)
        {
            CurrentValue = value;
            OnNewValue?.Invoke(value);
        }

        public IAwaiter<T> GetAwaiter() => 
            new NewValueNotifier<T>(this);
    }
}