using System;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Commands.CommandsInterfaces;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator), typeof(StopCommandExecutor))]
    public sealed class AttackCommandExecutor : CommandExecutorBase<IAttackCommand>
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        [SerializeField] private StopCommandExecutor _stopCommandExecutor;

        [Inject] private readonly IHealthHolder _ourHealth;
        [Inject(Id = "AttackDistance")] private readonly float _attackingDistance;
        [Inject(Id = "AttackPeriod")] private readonly int _attackingPeriod;

        private Vector3 _ourPosition;
        private Vector3 _targetPosition;

        private Transform _targetTransform;
        private AttackOperation _attackOperation;

        private readonly Subject<Vector3> _targetPositions = new();
        private readonly Subject<IAttackable> _attackTargets = new();

        private static readonly int _attackAnimation = Animator.StringToHash("Attack");
        private static readonly int _walkAnimation = Animator.StringToHash("Walk");
        private static readonly int _idleAnimation = Animator.StringToHash("Idle");

        public IHealthHolder OurHealth => _ourHealth;
        public float AttackingDistance => _attackingDistance;
        public int AttackingPeriod => _attackingPeriod;
        public Vector3 OurPosition => _ourPosition;
        public Vector3 TargetPosition => _targetPosition;
        public Subject<Vector3> TargetPositions => _targetPositions;
        public Subject<IAttackable> AttackTargets => _attackTargets;

        private void OnValidate()
        {
            _transform = transform;
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
        }

        private void Update()
        {
            if (_attackOperation == null)
            {
                return;
            }

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            _ourPosition = _transform.position;
            if (_targetTransform != null)
            {
                _targetPosition = _targetTransform.position;
            }
        }

        public override async Task ExecuteSpecificCommand(IAttackCommand command)
        {
            _targetTransform = (command.Target as Component).transform;
            _attackOperation = new(this, command.Target);
            _attackOperation.StartAttackAlgorithm();
            UpdatePosition();
            await _stopCommandExecutor.ExecuteOtherCommandWithCancellation(_attackOperation, CancelCommand);
        }

        private void CancelCommand()
        {
            _attackOperation.Cancel();
            _animator.SetTrigger(_idleAnimation);
            _attackOperation = null;
            _targetTransform = null;
        }

        private void StartAttackingTargets(IAttackable target)
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();
            _animator.SetTrigger(_attackAnimation);
            target.RecieveDamage(GetComponent<IDamageDealer>().Damage);
        }

        private void StartMovingToPosition(Vector3 position)
        {
            _navMeshAgent.destination = position;
            _animator.SetTrigger(_walkAnimation);
        }
    }
}