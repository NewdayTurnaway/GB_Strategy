using Abstractions;
using Abstractions.Commands;
using UnityEngine;
using UserControlSystem.CommandsRealization;
using Zenject;
using UniRx;
using UnityEngine.EventSystems;
using Abstractions.Commands.CommandsInterfaces;
using System.Threading.Tasks;

namespace UserControlSystem
{
    public sealed class MoveUnitCommandPresenter : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;

        [Inject] private readonly CommandButtonsModel _model;
        [Inject] private readonly SelectableValue _selectable;
        [Inject] private readonly Vector3Value _vector3Value;

        private ICommandsQueue _commandsQueue;
        private bool _blockInteraction;
        private bool _enableMoveCommand;

        private void OnValidate() =>
            _eventSystem ??= FindObjectOfType<EventSystem>();

        [Inject]
        private void Init()
        {
            _selectable.Subscribe(OnSelected);
            _model.OnCommandSent += UnblockInteraction;
            _model.OnCommandCancel += UnblockInteraction;
            _model.OnCommandAccepted += BlockInteraction;

            var availableUiFramesStream = Observable.EveryUpdate()
                .Where(_ => !_eventSystem.IsPointerOverGameObject());

            var rightClicksStream = availableUiFramesStream
                .Where(_ => Input.GetMouseButtonDown(1));

            rightClicksStream.Subscribe(async _ =>
            {
                if (_blockInteraction)
                {
                    return;
                }
                if (_enableMoveCommand)
                {
                    if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                    {
                        await Task.Run(() => _commandsQueue.Clear());
                        
                    }
                    _commandsQueue.EnqueueCommand(new MoveCommand(_vector3Value.CurrentValue));
                }
            });
        }

        private void UnblockInteraction() => 
            _blockInteraction = false;

        private void BlockInteraction(ICommandExecutor obj) => 
            _blockInteraction = true;

        private void OnSelected(ISelectable selectable)
        {
            if (selectable != null)
            {
                _commandsQueue = (selectable as Component).GetComponentInParent<ICommandsQueue>();
                _enableMoveCommand = (selectable as Component).GetComponentInParent<ICommandExecutor<IMoveCommand>>() != null;
            }
            else
            {
                _enableMoveCommand = false;
            }
        }
    }
}