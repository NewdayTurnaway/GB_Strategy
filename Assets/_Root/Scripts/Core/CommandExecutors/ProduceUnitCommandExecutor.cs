using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public sealed class ProduceUnitCommandExecutor : CommandExecutorBase<IProduceUnitCommand>
    {
        [Header("Unit Produce Settings")]
        [SerializeField] private Transform _unitsParent;
        [SerializeField] private int _timeDelayToProduce = 3000;

        private int _queueNumber = 0;

        public override async void ExecuteSpecificCommand(IProduceUnitCommand command)
        {
            if (_queueNumber < 0)
            {
                _queueNumber = 0;
            }
            _queueNumber++;
            Debug.Log($"{name} | Produce Unit: InProcess | Queue Number: {_queueNumber}");
            await Task.Delay(_queueNumber * _timeDelayToProduce);
            Instantiate(command.UnitPrefab,
                        new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10)),
                        Quaternion.identity,
                        _unitsParent);
            
            _queueNumber--;
            Debug.Log($"{name} | Produce Unit: Complete | Queue Number: {_queueNumber}");
        }
    }
}