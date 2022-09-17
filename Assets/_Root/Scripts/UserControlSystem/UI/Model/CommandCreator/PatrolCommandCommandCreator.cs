using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using UserControlSystem.CommandsRealization;
using Zenject;

namespace UserControlSystem
{
    public sealed class PatrolCommandCommandCreator : CancellableCommandCreatorBase<IPatrolCommand, Vector3>
    {
        [Inject] private SelectableValue _selectableValue;

        protected override IPatrolCommand CreateCommand(Vector3 argument) => 
            new PatrolCommand(_selectableValue.CurrentValue.PivotPoint.position, argument);
    }
}