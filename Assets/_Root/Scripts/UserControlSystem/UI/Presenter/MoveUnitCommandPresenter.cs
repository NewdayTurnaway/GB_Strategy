using Abstractions;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions.Commands;
using UnityEngine;
using UserControlSystem.CommandsRealization;
using Zenject;

namespace UserControlSystem
{
    public sealed class MoveUnitCommandPresenter : MonoBehaviour
    {
        [Inject] private readonly SelectableValue _selectable;
        [Inject] private readonly Vector3Value _vector3Value;

        private CommandExecutorBase<IMoveCommand> _moveCommandExecutor;
        private bool _enableMoveCommand;
        
        private void Start()
        {
            _selectable.OnNewValue += OnSelected;
            OnSelected(_selectable.CurrentValue);
        }

        private void Update()
        {
            if (!_enableMoveCommand)
            {
                return;
            }

            if (Input.GetMouseButtonUp(1))
            {
                _moveCommandExecutor.ExecuteCommand(new MoveCommand(_vector3Value.CurrentValue));
            }
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