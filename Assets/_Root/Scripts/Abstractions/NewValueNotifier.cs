using System;
using Utils;

namespace Abstractions
{
    public sealed class NewValueNotifier<TAwaited> : IAwaiter<TAwaited>
    {
        private readonly ValueBase<TAwaited> _valueBase;
        private TAwaited _result;
        
        private bool _isCompleted;
        private Action _continuation;
        
        public bool IsCompleted => _isCompleted;

        public NewValueNotifier(ValueBase<TAwaited> valueBase)
        {
            _valueBase = valueBase;
            _valueBase.OnNewValue += OnNewValue;
        }
        
        public TAwaited GetResult() => 
            _result;


        public void OnCompleted(Action continuation)
        {
            if (_isCompleted)
            {
                continuation?.Invoke();
            }
            else
            {
                _continuation = continuation;
            }
        }

        private void OnNewValue(TAwaited obj)
        {
            _valueBase.OnNewValue -= OnNewValue;
            _result = obj;
            _isCompleted = true;
            _continuation?.Invoke();
        }
    }
}