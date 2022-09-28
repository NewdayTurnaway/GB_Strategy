using Abstractions;
using Abstractions.Commands;
using UnityEngine;
using Zenject;

namespace Core
{
    [RequireComponent(typeof(AttackerParallelnfoUpdater), typeof(FactionMemberParallelInfoUpdater))]
    public class UnitInstaller : MonoInstaller
    {
        [SerializeField] private AttackerParallelnfoUpdater _attackerParallelnfoUpdater;
        [SerializeField] private FactionMemberParallelInfoUpdater _factionMemberParallelInfoUpdater;

        private void OnValidate()
        {
            _attackerParallelnfoUpdater ??= GetComponent<AttackerParallelnfoUpdater>();
            _factionMemberParallelInfoUpdater ??= GetComponent<FactionMemberParallelInfoUpdater>();
        }

        public override void InstallBindings()
        {
            Container.Bind<IHealthHolder>().FromComponentInChildren();
            Container.Bind<float>().WithId("AttackDistance").FromInstance(1.5f);
            Container.Bind<int>().WithId("AttackPeriod").FromInstance(1000);

            Container.Bind<IAutomaticAttacker>().FromComponentInChildren();
            Container.Bind<ITickable>()
                     .FromInstance(_attackerParallelnfoUpdater);
            Container.Bind<ITickable>()
                     .FromInstance(_factionMemberParallelInfoUpdater);
            Container.Bind<IFactionMember>().FromComponentInChildren();
            Container.Bind<ICommandsQueue>().FromComponentInChildren();
        }
    } 
}