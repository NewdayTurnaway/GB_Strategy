using System;
using System.Collections.Generic;
using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using UserControlSystem.CommandsRealization;
using UserControlSystem.UI.View;
using Utils;

namespace UserControlSystem.UI.Presenter
{
    public sealed class CommandButtonsPresenter : MonoBehaviour
    {
        [SerializeField] private SelectableValue _selectable;
        [SerializeField] private CommandButtonsView _view;
        [SerializeField] private AssetsContext _context;

        private ISelectable _currentSelectable;

        private void Start()
        {
            _selectable.OnSelected += ONSelected;
            ONSelected(_selectable.CurrentValue);

            _view.OnClick += ONButtonClick;
        }

        private void ONSelected(ISelectable selectable)
        {
            if (_currentSelectable == selectable)
            {
                return;
            }

            _currentSelectable = selectable;

            _view.Clear();

            if (selectable == null)
            {
                return;
            }

            var commandExecutors = new List<ICommandExecutor>();
            commandExecutors.AddRange((selectable as Component).GetComponentsInParent<ICommandExecutor>());
            _view.MakeLayout(commandExecutors);
        }

        private void ONButtonClick(ICommandExecutor commandExecutor)
        {
            if (RunCommand<IProduceUnitCommand>(commandExecutor, _context.Inject(new ProduceUnitCommandHeir())))
            {
                return;
            }

            if (RunCommand<IAttackCommand>(commandExecutor, _context.Inject(new AttackCommand())))
            {
                return;
            }

            if (RunCommand<IStopCommand>(commandExecutor, _context.Inject(new StopCommand())))
            {
                return;
            }

            if (RunCommand<IMoveCommand>(commandExecutor, _context.Inject(new MoveCommand())))
            {
                return;
            }

            if (RunCommand<IPatrolCommand>(commandExecutor, _context.Inject(new PatrolCommand())))
            {
                return;
            }

            throw new ApplicationException($"{nameof(CommandButtonsPresenter)}.{nameof(ONButtonClick)}: " +
                                           $"Unknown type of commands executor: {commandExecutor.GetType().FullName}!");
        }

        private bool RunCommand<T>(ICommandExecutor commandExecutor, T command) where T : ICommand
        {
            if (commandExecutor is CommandExecutorBase<T> classSpecificExecutor)
            {
                classSpecificExecutor.ExecuteSpecificCommand(command);
                return true;
            }

            return false;
        }
    }
}