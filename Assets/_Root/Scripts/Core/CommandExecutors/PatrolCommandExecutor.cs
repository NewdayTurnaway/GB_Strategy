using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;

namespace Core
{
    public sealed class PatrolCommandExecutor : CommandExecutorBase<IPatrolCommand>
    {
        public override void ExecuteSpecificCommand(IPatrolCommand command)
        {
            Debug.Log($"{name} | Patrol from {command.From} to {command.To}");
        }
    }
}