using Abstractions.Commands.CommandsInterfaces;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(UnitMovementStop))]
    [RequireComponent(typeof(StopCommandExecutor))]
    public sealed class MoveCommandExecutor : CommandExecutorBase<IMoveCommand>
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;

        [SerializeField] private UnitMovementStop _movementStop;
        [SerializeField] private StopCommandExecutor _stopCommandExecutor;

        private static readonly int _walkAnimation = Animator.StringToHash("Walk");
        private static readonly int _idleAnimation = Animator.StringToHash("Idle");

        private void OnValidate()
        {
            _navMeshAgent ??= GetComponent<NavMeshAgent>();
            _animator ??= GetComponent<Animator>();
            _movementStop ??= GetComponent<UnitMovementStop>();
            _stopCommandExecutor ??= GetComponent<StopCommandExecutor>();
        }

        public override async Task ExecuteSpecificCommand(IMoveCommand command)
        {
            _navMeshAgent.destination = command.Target;
            _animator.SetTrigger(_walkAnimation);
            await _stopCommandExecutor.ExecuteOtherCommandWithCancellation(_movementStop, CancelCommand);
            _animator.SetTrigger(_idleAnimation);
        }

        private void CancelCommand()
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();
        }
    }
}