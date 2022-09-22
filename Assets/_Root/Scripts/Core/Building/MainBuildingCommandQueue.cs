using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using Zenject;

namespace Core
{
    public class MainBuildingCommandQueue: MonoBehaviour, ICommandsQueue
    {
        [Inject] private readonly CommandExecutorBase<IProduceUnitCommand> _produceUnitCommandExecutor;

        public void Clear() { }

        public async void EnqueueCommand(object command)
        {
            await _produceUnitCommandExecutor.TryExecuteCommand(command);
        }
    }
}