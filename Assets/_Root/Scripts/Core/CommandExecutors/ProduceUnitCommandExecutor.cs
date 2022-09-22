using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core
{
    public sealed class ProduceUnitCommandExecutor : CommandExecutorBase<IProduceUnitCommand>, IUnitProducer
    {
        [SerializeField] private Transform _unitsParent;
        [SerializeField] private int _queueLimit = 5;

        [Inject] private DiContainer _diContainer;
        private readonly ReactiveCollection<IUnitProductionTask> _queue = new();
     
        public IReadOnlyReactiveCollection<IUnitProductionTask> Queue => _queue;

        private void Start() => 
            Observable.EveryUpdate().Subscribe(_ => OnUpdate());

        public void Cancel(int index) => 
            RemoveTaskAtIndex(index);

        public override Task ExecuteSpecificCommand(IProduceUnitCommand command)
        {
            if (_queue.Count == _queueLimit)
            {
                return Task.CompletedTask;
            }

            _queue.Add(new UnitProductionTask(command.UnitPrefab, command.UnitName, command.Icon, command.ProduceTime));
            return Task.CompletedTask;
        }

        private void OnUpdate()
        {
            if (_queue.Count == 0)
            {
                return;
            }

            var innerTask = (UnitProductionTask)_queue[0];
            innerTask.TimeLeft -= Time.deltaTime;

            if (innerTask.TimeLeft <= 0)
            {
                RemoveTaskAtIndex(0);
                var instance = _diContainer.InstantiatePrefab(innerTask.UnitPrefab, transform.position, Quaternion.identity, _unitsParent);
                var queue = instance.GetComponent<ICommandsQueue>();
                var building = GetComponent<MainBuilding>();
                queue.EnqueueCommand(new MoveCommand(building.Destination));
            }
        }

        private void RemoveTaskAtIndex(int index)
        {
            for (int i = index; i < _queue.Count - 1; i++)
            {
                _queue[i] = _queue[i + 1];
            }

            _queue.RemoveAt(_queue.Count - 1);
        }
    }
}