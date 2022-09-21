using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UniRx;
using UnityEngine;

namespace Core
{
    public sealed class ProduceUnitCommandExecutor : CommandExecutorBase<IProduceUnitCommand>, IUnitProducer
    {
        [SerializeField] private Transform _unitsParent;
        [SerializeField] private int _queueLimit = 5;

        private readonly ReactiveCollection<IUnitProductionTask> _queue = new();
     
        public IReadOnlyReactiveCollection<IUnitProductionTask> Queue => _queue;

        private void Start() => 
            Observable.EveryUpdate().Subscribe(_ => OnUpdate());

        public void Cancel(int index) => 
            RemoveTaskAtIndex(index);

        public override void ExecuteSpecificCommand(IProduceUnitCommand command)
        {
            if (_queue.Count == _queueLimit)
            {
                return;
            }

            _queue.Add(new UnitProductionTask(command.UnitPrefab, command.UnitName, command.Icon, command.ProduceTime));
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
                Instantiate(innerTask.UnitPrefab,
                            new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)),
                            Quaternion.identity,
                            _unitsParent);
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