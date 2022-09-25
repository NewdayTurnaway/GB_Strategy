using System;
using System.Threading;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Commands.CommandsInterfaces;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Zenject;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator), typeof(StopCommandExecutor))]
    public sealed class AttackCommandExecutor : CommandExecutorBase<IAttackCommand>
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        [SerializeField] private StopCommandExecutor _stopCommandExecutor;

        [Inject] private readonly IHealthHolder _ourHealth;
        [Inject(Id = "AttackDistance")] private readonly float _attackingDistance;
        [Inject(Id = "AttackPeriod")] private readonly int _attackingPeriod;

        private Vector3 _ourPosition;
        private Vector3 _targetPosition;
        private Quaternion _ourRotation;

        private Transform _targetTransform;
        private AttackOperation _attackOperation;

        private readonly Subject<Vector3> _targetPositions = new();
        private readonly Subject<Quaternion> _targetRotations = new();
        private readonly Subject<IAttackable> _attackTargets = new();

        private static readonly int _attackAnimation = Animator.StringToHash("Attack");
        private static readonly int _walkAnimation = Animator.StringToHash("Walk");
        private static readonly int _idleAnimation = Animator.StringToHash("Idle");

        private void OnValidate()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _stopCommandExecutor = GetComponent<StopCommandExecutor>();
        }

        [Inject]
        private void Init()
        {
            _targetPositions
                .Select(value => new Vector3((float)Math.Round(value.x, 2),
                                             (float)Math.Round(value.y, 2),
                                             (float)Math.Round(value.z, 2)))
                .Distinct()
                .ObserveOnMainThread()
                .Subscribe(StartMovingToPosition);

            _attackTargets
                .ObserveOnMainThread()
                .Subscribe(StartAttackingTargets);

            _targetRotations
                .ObserveOnMainThread()
                .Subscribe(SetAttackRotation);

            //UpdatePosition(); // if enabled - application freeze
        }

        private void Update()
        {
            if (_attackOperation == null)
            {
                return;
            }
            //Has time to work only in debug mode, after setting a breakpoint in the stream and pressing the F5 button
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            //lock (this)
            {
                _ourPosition = transform.position;
                _ourRotation = transform.rotation;
                if (_targetTransform != null)
                {
                    _targetPosition = _targetTransform.position;
                }
            }
        }

        public override async Task ExecuteSpecificCommand(IAttackCommand command)
        {
            _targetTransform = (command.Target as Component).transform;
            _attackOperation = new(this, command.Target);
            //UpdatePosition(); // if enabled - work only StartMovingToPosition | if disabled - work SetAttackRotation & StartMovingToPosition
            _stopCommandExecutor.CancellationTokenSource = new();

            try
            {
                await _attackOperation.WithCancellation(_stopCommandExecutor.CancellationTokenSource.Token);
            }
            catch
            {
                _attackOperation.Cancel();
            }

            _animator.SetTrigger(_idleAnimation);
            _attackOperation = null;
            _targetTransform = null;
            _stopCommandExecutor.CancellationTokenSource = null;
        }

        private void SetAttackRotation(Quaternion targetRotation)
        {
            Debug.Log(nameof(SetAttackRotation));
            transform.rotation = targetRotation;
        }

        private void StartAttackingTargets(IAttackable target)
        {
            Debug.Log(nameof(StartAttackingTargets));
            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();
            _animator.SetTrigger(_attackAnimation);
            target.RecieveDamage(GetComponent<IDamageDealer>().Damage);
        }

        private void StartMovingToPosition(Vector3 position)
        {
            Debug.Log(nameof(StartMovingToPosition));
            _navMeshAgent.destination = position;
            _animator.SetTrigger(_walkAnimation);
        }

        #region AttackOperation

        public sealed class AttackOperation : IAwaitable<AsyncExtensions.Void>
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

            private event Action OnComplete;

            private readonly AttackCommandExecutor _attackCommandExecutor;
            private readonly IAttackable _target;

            private bool _isCancelled;

            public AttackOperation(AttackCommandExecutor attackCommandExecutor, IAttackable target)
            {
                _target = target;
                _attackCommandExecutor = attackCommandExecutor;

                var thread = new Thread(AttackAlgorythm);
                thread.Start();
            }

            public void Cancel()
            {
                _isCancelled = true;
                OnComplete?.Invoke();
            }

            private void AttackAlgorythm(object obj)
            {
                while (true) // Breakpoint
                {
                    if (_attackCommandExecutor == null
                        || _attackCommandExecutor._ourHealth.Health.CurrentHealth == 0
                        || _target.Health.CurrentHealth == 0
                        || _isCancelled)
                    {
                        OnComplete?.Invoke();
                        return;
                    }

                    var targetPosition = default(Vector3);
                    var ourPosition = default(Vector3);
                    var ourRotation = default(Quaternion);
                    //lock (_attackCommandExecutor)
                    {
                        //
                        targetPosition = _attackCommandExecutor._targetPosition;
                        ourPosition = _attackCommandExecutor._ourPosition;
                        ourRotation = _attackCommandExecutor._ourRotation;
                    }

                    var vector = targetPosition - ourPosition;
                    var distanceToTarget = vector.magnitude;
                    if (distanceToTarget > _attackCommandExecutor._attackingDistance)
                    {
                        var finalDestination = targetPosition
                            - vector.normalized
                            * (_attackCommandExecutor._attackingDistance * 0.9f);
                        _attackCommandExecutor._targetPositions.OnNext(finalDestination);
                        Thread.Sleep(100);
                    }
                    else if (ourRotation != Quaternion.LookRotation(vector))
                    {
                        _attackCommandExecutor._targetRotations.OnNext(Quaternion.LookRotation(vector));
                    }
                    else
                    {
                        _attackCommandExecutor._attackTargets.OnNext(_target);
                        Thread.Sleep(_attackCommandExecutor._attackingPeriod);
                    }
                }
            }

            public IAwaiter<AsyncExtensions.Void> GetAwaiter() =>
                new AttackOperationAwaiter(this);
        }

        #endregion
    }
}