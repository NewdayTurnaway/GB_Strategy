using Abstractions;
using Zenject;

namespace Core
{
    public class UnitInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IHealthHolder>().FromComponentInChildren();
            Container.Bind<float>().WithId("AttackDistance").FromInstance(1.5f);
            Container.Bind<int>().WithId("AttackPeriod").FromInstance(1000);
        }
    } 
}