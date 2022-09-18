using Utils;

namespace Abstractions
{
    public sealed class NewValueNotifier<TAwaited> : AwaiterBase<TAwaited>
    {
        private readonly ValueBase<TAwaited> _valueBase;

        public NewValueNotifier(ValueBase<TAwaited> valueBase)
        {
            _valueBase = valueBase;
            _valueBase.OnNewValue += OnNewValue;
        }

        private void OnNewValue(TAwaited obj)
        {
            _valueBase.OnNewValue -= OnNewValue;
            OnWaitFinish(obj);
        }
    }
}