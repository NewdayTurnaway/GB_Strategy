using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using Zenject;

namespace Core
{
    public class HealingBuildingCommandQueue: MonoBehaviour, ICommandsQueue
    {
        [Inject] private readonly CommandExecutorBase<IHealingCommand> _healingCommandExecutor;

        public ICommand CurrentCommand => default;

        public void Clear() { }

        public async void EnqueueCommand(object command) => 
            await _healingCommandExecutor.TryExecuteCommand(command);
    }
}