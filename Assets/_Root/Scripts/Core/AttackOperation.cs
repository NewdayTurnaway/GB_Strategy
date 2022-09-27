using Abstractions;
using System;
using System.Threading;
using Utils;

namespace Core
{
    public class AttackOperation : IAwaitable<AsyncExtensions.Void>
    {
        private readonly AttackCommandExecutor _attackCommandExecutor;
        private readonly IAttackable _target;

        private bool _isCancelled;

        public event Action OnComplete;

        public AttackOperation(AttackCommandExecutor attackCommandExecutor, IAttackable target)
        {
            _target = target;
            _attackCommandExecutor = attackCommandExecutor;
        }

        public void StartAttackAlgorithm()
        {
            var thread = new Thread(AttackAlgorithm);
            thread.Start();
        }

        public void Cancel()
        {
            _isCancelled = true;
            OnComplete?.Invoke();
        }
        
        private void AttackAlgorithm(object obj)
        {
            while (!_isCancelled)
            {
                if (_attackCommandExecutor == null
                    || _attackCommandExecutor.OurHealth.Health.CurrentHealth == 0
                    || _target.Health.CurrentHealth == 0)
                {
                    OnComplete?.Invoke();
                    return;
                }

                var targetPosition = _attackCommandExecutor.TargetPosition;
                var ourPosition = _attackCommandExecutor.OurPosition;

                var vector = targetPosition - ourPosition;
                var distanceToTarget = vector.magnitude;
                if (distanceToTarget > _attackCommandExecutor.AttackingDistance)
                {
                    var finalDestination = targetPosition
                                           - vector.normalized
                                           * (_attackCommandExecutor.AttackingDistance * 0.9f);
                    _attackCommandExecutor.TargetPositions.OnNext(finalDestination);
                    Thread.Sleep(100);
                }
                else
                {
                    _attackCommandExecutor.AttackTargets.OnNext(_target);
                    Thread.Sleep(_attackCommandExecutor.AttackingPeriod);
                }
            }
        }

        public IAwaiter<AsyncExtensions.Void> GetAwaiter() =>
            new AttackOperationAwaiter(this);
    }
}