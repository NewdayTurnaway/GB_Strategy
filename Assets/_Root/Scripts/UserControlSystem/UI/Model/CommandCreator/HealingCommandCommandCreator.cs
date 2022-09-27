using System;
using Abstractions.Commands.CommandsInterfaces;
using UserControlSystem.CommandsRealization;

namespace UserControlSystem
{
    public sealed class HealingCommandCommandCreator : CommandCreatorBase<IHealingCommand>
    {
        protected override void ClassSpecificCommandCreation(Action<IHealingCommand> creationCallback) => 
            creationCallback?.Invoke(new HealingCommand());
    }
}