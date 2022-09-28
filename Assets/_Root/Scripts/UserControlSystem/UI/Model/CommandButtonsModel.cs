using System;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using Zenject;

namespace UserControlSystem
{
    public sealed class CommandButtonsModel
    {
        public event Action<ICommandExecutor> OnCommandAccepted;
        public event Action OnCommandSent;
        public event Action OnCommandCancel;

        [Inject] private readonly CommandCreatorBase<IProduceUnitCommand> _unitProducer;
        [Inject] private readonly CommandCreatorBase<IAttackCommand> _attacker;
        [Inject] private readonly CommandCreatorBase<IStopCommand> _stopper;
        [Inject] private readonly CommandCreatorBase<IMoveCommand> _mover;
        [Inject] private readonly CommandCreatorBase<IPatrolCommand> _patroller;
        [Inject] private readonly CommandCreatorBase<ISetDestinationCommand> _setDestination;
        [Inject] private readonly CommandCreatorBase<IHealingCommand> _healer;
        [Inject] private readonly CommandCreatorBase<ITeleportCommand> _teleporter;

        private bool _commandIsPending;

        public void OnCommandButtonClicked(ICommandExecutor commandExecutor, ICommandsQueue queue)
        {
            if (_commandIsPending)
            {
                ProcessOnCancel();
            }
            _commandIsPending = true;
            OnCommandAccepted?.Invoke(commandExecutor);

            _unitProducer.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, queue));
            _attacker.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, queue));
            _stopper.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, queue));
            _mover.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, queue));
            _patroller.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, queue));
            _setDestination.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, queue));
            _healer.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, queue));
            _teleporter.ProcessCommandExecutor(commandExecutor, command => ExecuteCommandWrapper(command, queue));
        }

        public async void ExecuteCommandWrapper(object command, ICommandsQueue commandsQueue)
        {
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                await Task.Run(() => commandsQueue.Clear());
            }
            commandsQueue.EnqueueCommand(command);
            _commandIsPending = false;
            OnCommandSent?.Invoke();
        }

        public void OnSelectionChanged()
        {
            _commandIsPending = false;
            ProcessOnCancel();
        }

        private void ProcessOnCancel()
        {
            _unitProducer.ProcessCancel();
            _attacker.ProcessCancel();
            _stopper.ProcessCancel();
            _mover.ProcessCancel();
            _patroller.ProcessCancel();
            _setDestination.ProcessCancel();
            _healer.ProcessCancel();
            _teleporter.ProcessCancel();

            OnCommandCancel?.Invoke();
        }
    }
}