using System;
using UnityEngine;

namespace UserControlSystem
{
    public abstract class ValueBase<T> : ScriptableObject
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