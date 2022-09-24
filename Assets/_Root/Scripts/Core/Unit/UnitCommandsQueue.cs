using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core
{
    public sealed class UnitCommandsQueue : MonoBehaviour, ICommandsQueue
    {
        [Inject] private readonly CommandExecutorBase<IMoveCommand> _moveCommandExecutor;
        [Inject] private readonly CommandExecutorBase<IPatrolCommand> _patrolCommandExecutor;
        [Inject] private readonly CommandExecutorBase<IAttackCommand> _attackCommandExecutor;
        [Inject] private readonly CommandExecutorBase<IStopCommand> _stopCommandExecutor;
        
        private readonly ReactiveCollection<ICommand> _innerCollection = new();

        [Inject]
        private void Init() => 
            _innerCollection.ObserveAdd().Subscribe(OnNewCommand).AddTo(this);

        private void OnNewCommand(ICommand command, int index)
        {
            if (index == 0)
            {
                ExecuteCommand(command);
            }
        }

        private async void ExecuteCommand(ICommand command)
        {
            await _moveCommandExecutor.TryExecuteCommand(command);
            await _patrolCommandExecutor.TryExecuteCommand(command);
            await _attackCommandExecutor.TryExecuteCommand(command);
            await _stopCommandExecutor.TryExecuteCommand(command);
            if (_innerCollection.Count > 0)
            {
                _innerCollection.RemoveAt(0);
            }
            CheckTheQueue();
        }

        private void CheckTheQueue()
        {
            if (_innerCollection.Count > 0)
            {
                ExecuteCommand(_innerCollection[0]);
            }
        }

        public void EnqueueCommand(object wrappedCommand)
        {
            var command = wrappedCommand as ICommand;
            _innerCollection.Add(command);
        }

        public void Clear()
        {
            _innerCollection.Clear();
            _stopCommandExecutor.ExecuteSpecificCommand(new StopCommand());
        }
    }
}