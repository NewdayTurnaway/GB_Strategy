using System;

namespace UserControlSystem
{
    public abstract class ValueBase<T>
    {
        public T CurrentValue { get; private set; }
        public event Action<T> OnNewValue;

        public void SetValue(T value)
        {
            CurrentValue = value;
            OnNewValue?.Invoke(value);
        }
    }
}