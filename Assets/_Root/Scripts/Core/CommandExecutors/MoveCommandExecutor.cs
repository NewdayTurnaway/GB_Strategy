using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(UnitMovementStop))]
    public sealed class MoveCommandExecutor : CommandExecutorBase<IMoveCommand>
    {
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private UnitMovementStop _stop;

        private static readonly int _walkAnimation = Animator.StringToHash("Walk");
        private static readonly int _idleAnimation = Animator.StringToHash("Idle");

        private void OnValidate() => 
            InstallFields();

        private void Awake() => 
            InstallFields();

        private void InstallFields()
        {
            _navMeshAgent ??= GetComponent<NavMeshAgent>();
            _animator ??= GetComponent<Animator>();
            _stop ??= GetComponent<UnitMovementStop>();
        }

        public override async void ExecuteSpecificCommand(IMoveCommand command)
        {
            _navMeshAgent.destination = command.Target;
            _animator.SetTrigger(_walkAnimation);
            await _stop;
            _animator.SetTrigger(_idleAnimation);
        }
    }
}