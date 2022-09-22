using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

namespace UserControlSystem.UI.View
{
    public sealed class CommandButtonsView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _moveButton;
        [SerializeField] private Button _patrolButton;
        [SerializeField] private Button _stopButton;
        [SerializeField] private Button _produceUnitButton;
        [SerializeField] private Button _setDestinationButton;

        private Dictionary<Type, Button> _buttonsByExecutorType;

        public Action<ICommandExecutor, ICommandsQueue> OnClick;
        
        private void OnValidate() =>
            _rectTransform ??= (RectTransform)gameObject.transform;

        private void Start()
        {
            _buttonsByExecutorType = new Dictionary<Type, Button>
            {
                { typeof(ICommandExecutor<IAttackCommand>), _attackButton },
                { typeof(ICommandExecutor<IMoveCommand>), _moveButton },
                { typeof(ICommandExecutor<IPatrolCommand>), _patrolButton },
                { typeof(ICommandExecutor<IStopCommand>), _stopButton },
                { typeof(ICommandExecutor<IProduceUnitCommand>), _produceUnitButton },
                { typeof(ICommandExecutor<ISetDestinationCommand>), _setDestinationButton }
            };
        }

        public void UnblockAllInteractions() => 
            SetInteractible(true);

        public void BlockInteractions(ICommandExecutor commandExecutor)
        {
            UnblockAllInteractions();
            GetButtonByType(commandExecutor.GetType()).interactable = false;
        }

        public void MakeLayout(List<ICommandExecutor> commandExecutors, ICommandsQueue queue)
        {
            for (int i = 0; i < commandExecutors.Count; i++)
            {
                var currentExecutor = commandExecutors[i];
                var button = GetButtonByType(currentExecutor.GetType());
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() => OnClick?.Invoke(currentExecutor, queue));
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }

        public void Clear()
        {
            foreach (var kvp in _buttonsByExecutorType)
            {
                kvp.Value.onClick.RemoveAllListeners();
                kvp.Value.gameObject.SetActive(false);
            }
        }

        private void SetInteractible(bool value)
        {
            _attackButton.interactable = value;
            _moveButton.interactable = value;
            _patrolButton.interactable = value;
            _stopButton.interactable = value;
            _produceUnitButton.interactable = value;
            _setDestinationButton.interactable = value;
        }

        private Button GetButtonByType(Type executorInstanceType)
        {
            return _buttonsByExecutorType
                    .First(type => type
                        .Key
                        .IsAssignableFrom(executorInstanceType))
                    .Value;
        }
    }
}