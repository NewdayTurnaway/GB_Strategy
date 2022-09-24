using Abstractions.Commands.CommandsInterfaces;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public sealed class AttackCommandExecutor : CommandExecutorBase<IAttackCommand>
    {
        public override Task ExecuteSpecificCommand(IAttackCommand command)
        {
            Debug.Log($"{name} | Attack the {command.Target.PivotPoint.name} with {command.Target.Health.CurrentHealth} HP");
            return Task.CompletedTask;
        }
    }
}