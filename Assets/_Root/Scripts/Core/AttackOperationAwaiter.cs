using Utils;

namespace Core
{
    public class AttackOperationAwaiter : AwaiterBase<AsyncExtensions.Void>
    {
        private readonly AttackOperation _attackOperation;

        public AttackOperationAwaiter(AttackOperation attackOperation)
        {
            _attackOperation = attackOperation;
            attackOperation.OnComplete += OnComplete;
        }

        private void OnComplete()
        {
            _attackOperation.OnComplete -= OnComplete;
            OnWaitFinish(new AsyncExtensions.Void());
        }
    }
}