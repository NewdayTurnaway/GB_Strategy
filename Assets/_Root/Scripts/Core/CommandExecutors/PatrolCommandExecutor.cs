using Abstractions.Commands.CommandsInterfaces;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public sealed class PatrolCommandExecutor : CommandExecutorBase<IPatrolCommand>
    {
        public override Task ExecuteSpecificCommand(IPatrolCommand command)
        {
            Debug.Log($"{name} | Patrol from {command.From} to {command.To}");
            return Task.CompletedTask;
        }
    }
}