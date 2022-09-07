using System;
using Abstractions;
using Abstractions.Commands.CommandsInterfaces;
using UserControlSystem.CommandsRealization;
using Utils;
using Zenject;

namespace UserControlSystem
{
    public sealed class AttackCommandCommandCreator : CommandCreatorBase<IAttackCommand>
    {
        [Inject] private AssetsContext _context;
        private Action<IAttackCommand> _creationCallback;

        [Inject]
        private void Init(AttackableValue groundClicks) 
            => groundClicks.OnAttacked += ONNewValue;

        private void ONNewValue(IAttackable attackable)
        {
            _creationCallback?.Invoke(_context.Inject(new AttackCommand(attackable)));
            _creationCallback = null;
        }

        protected override void ClassSpecificCommandCreation(Action<IAttackCommand> creationCallback)
            => _creationCallback = creationCallback;

        public override void ProcessCancel()
        {
            base.ProcessCancel();
            _creationCallback = null;
        }
    }
}