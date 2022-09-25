namespace Abstractions
{
    public interface IAttackable : ISelectable
    {
        void RecieveDamage(int amount);
    }
}