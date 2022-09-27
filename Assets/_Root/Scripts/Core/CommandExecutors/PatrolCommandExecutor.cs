using Abstractions.Commands.CommandsInterfaces;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(UnitMovementStop))]
    [RequireComponent(typeof(StopCommandExecutor))]
    public sealed class PatrolCommandExecutor : CommandExecutorBase<IPatrolCommand>
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;

        [SerializeField] private UnitMovementStop _movementStop;
        [SerializeField] private StopCommandExecutor _stopCommandExecutor;

        private static readonly int _walkAnimation = Animator.StringToHash("Walk");
        private static readonly int _idleAnimation = Animator.StringToHash("Idle");

        private bool _inPatrol;

        private void OnValidate()
        {
            _navMeshAgent ??= GetComponent<NavMeshAgent>();
            _animator ??= GetComponent<Animator>();
            _movementStop ??= GetComponent<UnitMovementStop>();
            _stopCommandExecutor ??= GetComponent<StopCommandExecutor>();
        }

        public override async Task ExecuteSpecificCommand(IPatrolCommand command)
        {
            var point1 = command.From;
            var point2 = command.To;
            _animator.SetTrigger(_walkAnimation);
            _inPatrol = true;
            while (_inPatrol)
            {
                _navMeshAgent.destination = point2;
                await _stopCommandExecutor.ExecuteOtherCommandWithCancellation(_movementStop, CancelCommand);
                (point1, point2) = (point2, point1);
            }
        }

        private void CancelCommand()
        {
            _inPatrol = false;
            _animator.SetTrigger(_idleAnimation);
            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();
        }
    }
}