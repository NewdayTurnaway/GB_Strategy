using System.Threading.Tasks;
using Abstractions.Commands.CommandsInterfaces;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(UnitMovementStop))]
    [RequireComponent(typeof(StopCommandExecutor))]
    public sealed class TeleportCommandExecutor : CommandExecutorBase<ITeleportCommand>
    {
        [SerializeField] private Button _teleportButton;
        [SerializeField] private float _teleportDelay = 5f;

        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;

        [SerializeField] private UnitMovementStop _movementStop;
        [SerializeField] private StopCommandExecutor _stopCommandExecutor;

        private static readonly int _teleportAnimation = Animator.StringToHash("Teleport");
        private static readonly int _idleAnimation = Animator.StringToHash("Idle");

        private float _timeLeft;
        private bool _enableButton = true;

        private void OnValidate()
        {
            _navMeshAgent ??= GetComponent<NavMeshAgent>();
            _animator ??= GetComponent<Animator>();
            _movementStop ??= GetComponent<UnitMovementStop>();
            _stopCommandExecutor ??= GetComponent<StopCommandExecutor>();
        }

        private void Start() =>
            Observable.EveryUpdate().Subscribe(_ => OnUpdate());

        public override async Task ExecuteSpecificCommand(ITeleportCommand command)
        {
            _navMeshAgent.Warp(command.Target);
            _animator.SetTrigger(_teleportAnimation);
            await _stopCommandExecutor.ExecuteOtherCommandWithCancellation(_movementStop, CancelCommand);
            _animator.SetTrigger(_idleAnimation);
            _timeLeft = _teleportDelay;
            _enableButton = false;
            _teleportButton.interactable = _enableButton;
        }

        private void CancelCommand()
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();
        }

        private void OnUpdate()
        {
            if (_enableButton)
            {
                return;
            }

            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                _enableButton = true;
                _teleportButton.interactable = _enableButton;
            }
        }
    }
}