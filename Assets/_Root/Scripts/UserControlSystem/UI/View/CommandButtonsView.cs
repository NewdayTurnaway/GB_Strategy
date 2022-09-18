using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
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

        private Dictionary<Type, Button> _buttonsByExecutorType;

        public Action<ICommandExecutor> OnClick;
        
        private void OnValidate() =>
            _rectTransform ??= (RectTransform)gameObject.transform;

        private void Start()
        {
            _buttonsByExecutorType = new Dictionary<Type, Button>
            {
                { typeof(CommandExecutorBase<IAttackCommand>), _attackButton },
                { typeof(CommandExecutorBase<IMoveCommand>), _moveButton },
                { typeof(CommandExecutorBase<IPatrolCommand>), _patrolButton },
                { typeof(CommandExecutorBase<IStopCommand>), _stopButton },
                { typeof(CommandExecutorBase<IProduceUnitCommand>), _produceUnitButton }
            };
        }

        public void UnblockAllInteractions() => 
            SetInteractible(true);

        public void BlockInteractions(ICommandExecutor commandExecutor)
        {
            UnblockAllInteractions();
            GetButtonByType(commandExecutor.GetType()).interactable = false;
        }

        public void MakeLayout(List<ICommandExecutor> commandExecutors)
        {
            for (int i = 0; i < commandExecutors.Count; i++)
            {
                var currentExecutor = commandExecutors[i];
                var button = GetButtonByType(currentExecutor.GetType());
                button.gameObject.SetActive(true);
                button.onClick.AddListener(() => OnClick?.Invoke(currentExecutor));
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