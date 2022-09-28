using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using Zenject;

namespace Core
{
    public class MainBuildingCommandQueue: MonoBehaviour, ICommandsQueue
    {
        [Inject] private readonly CommandExecutorBase<IProduceUnitCommand> _produceUnitCommandExecutor;
        [Inject] private readonly CommandExecutorBase<ISetDestinationCommand> _setDestinationCommandExecutor;

        public ICommand CurrentCommand => default;

        public void Clear() { }

        public async void EnqueueCommand(object command)
        {
            await _produceUnitCommandExecutor.TryExecuteCommand(command);
            await _setDestinationCommandExecutor.TryExecuteCommand(command);
        }
    }
}