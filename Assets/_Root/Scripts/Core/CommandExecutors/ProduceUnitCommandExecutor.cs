using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;

namespace Core
{
    public sealed class ProduceUnitCommandExecutor : CommandExecutorBase<IProduceUnitCommand>
    {
        [Header("Unit Produce Settings")]
        [SerializeField] private Transform _unitsParent;

        public override void ExecuteSpecificCommand(IProduceUnitCommand command) 
            => Instantiate(command.UnitPrefab,
                           new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10)),
                           Quaternion.identity,
                           _unitsParent);
    }
}