using Abstractions;
using UnityEngine;
using UserControlSystem.CommandsRealization;

namespace UserControlSystem
{
    public sealed class SetDestinationCommandCommandCreator : CancellableCommandCreatorBase<ISetDestinationCommand, Vector3>
    {
        protected override ISetDestinationCommand CreateCommand(Vector3 argument) => 
            new SetDestinationCommand(argument);
    }
}