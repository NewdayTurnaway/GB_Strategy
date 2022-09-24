using Abstractions;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public sealed class SetDestinationCommandExecutor : CommandExecutorBase<ISetDestinationCommand>
    {
        [SerializeField] private MainBuilding _mainBuilding;

        private void OnValidate() => 
            _mainBuilding ??= GetComponent<MainBuilding>();

        public override Task ExecuteSpecificCommand(ISetDestinationCommand command)
        {
            _mainBuilding.Destination = command.Destination;
            return Task.CompletedTask;
        }
    }
}