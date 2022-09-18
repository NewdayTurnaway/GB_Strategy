using Abstractions;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;
using UnityEngine;
using UserControlSystem.CommandsRealization;
using Zenject;
using UniRx;
using UnityEngine.EventSystems;

namespace UserControlSystem
{
    public sealed class MoveUnitCommandPresenter : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;

        [Inject] private readonly SelectableValue _selectable;
        [Inject] private readonly Vector3Value _vector3Value;

        private CommandExecutorBase<IMoveCommand> _moveCommandExecutor;
        private bool _enableMoveCommand;

        private void OnValidate() =>
            _eventSystem ??= FindObjectOfType<EventSystem>();

        [Inject]
        private void Init()
        {
            _selectable.OnNewValue += OnSelected;
            OnSelected(_selectable.CurrentValue);

            var availableUiFramesStream = Observable.EveryUpdate()
                .Where(_ => !_eventSystem.IsPointerOverGameObject());

            var rightClicksStream = availableUiFramesStream
                .Where(_ => Input.GetMouseButtonDown(1));

            rightClicksStream.Subscribe(_ =>
            {
                if (_enableMoveCommand)
                {
                    _moveCommandExecutor.ExecuteCommand(new MoveCommand(_vector3Value.CurrentValue));
                }
            });
        }

        private void OnSelected(ISelectable selectable)
        {
            if (selectable != null)
            {
                _moveCommandExecutor = (selectable as Component).GetComponentInParent<CommandExecutorBase<IMoveCommand>>();
                if (_moveCommandExecutor != null)
                {
                    _enableMoveCommand = true;
                }
                else
                {
                    _enableMoveCommand = false;
                }
            }
            else
            {
                _enableMoveCommand = false;
            }
        }
    }
}