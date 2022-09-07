using Abstractions;
using Abstractions.Commands.CommandsInterfaces;

namespace UserControlSystem.CommandsRealization
{
    public sealed class AttackCommand : IAttackCommand
    {
        public IAttackable Target { get; }
        
        public AttackCommand(IAttackable target)
        {
            Target = target;
        }
    }
}