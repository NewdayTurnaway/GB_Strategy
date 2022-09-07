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
                return;

            _currentSelectable = selectable;

            _view.Clear();

            if (selectable == null)
                return;

            List<ICommandExecutor> commandExecutors = new();
            commandExecutors.AddRange((selectable as Component).GetComponentsInParent<ICommandExecutor>());
            _view.MakeLayout(commandExecutors);
        }

        private void ONButtonClick(ICommandExecutor commandExecutor)
        {
            if (CommandRun<IProduceUnitCommand>(commandExecutor, _context.Inject(new ProduceUnitCommandHeir()))) 
                return;
            if (CommandRun<IAttackCommand>(commandExecutor, _context.Inject(new AttackCommand()))) 
                return;
            if (CommandRun<IStopCommand>(commandExecutor, _context.Inject(new StopCommand()))) 
                return;
            if (CommandRun<IMoveCommand>(commandExecutor, _context.Inject(new MoveCommand()))) 
                return;
            if (CommandRun<IPatrolCommand>(commandExecutor, _context.Inject(new PatrolCommand()))) 
                return;

            throw new ApplicationException($"{nameof(CommandButtonsPresenter)}.{nameof(ONButtonClick)}: " +
                                           $"Unknown type of commands executor: {commandExecutor.GetType().FullName}!");
        }

        private bool CommandRun<T>(ICommandExecutor commandExecutor, T command) where T : ICommand
        {
            CommandExecutorBase<T> classSpecificExecutor = commandExecutor as CommandExecutorBase<T>;

            bool isExist = classSpecificExecutor != null;
            if (isExist)
                classSpecificExecutor.ExecuteSpecificCommand(command);

            return isExist;
        }
    }
}