using Abstractions;
using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using Zenject;

namespace UserControlSystem
{
    public class UIModelInstaller : MonoInstaller
    {
        [SerializeField] private Sprite _unitSprite;

        public override void InstallBindings()
        {
            Container.Bind<CommandCreatorBase<IProduceUnitCommand>>()
                .To<ProduceUnitCommandCommandCreator>().AsSingle();
            Container.Bind<CommandCreatorBase<IAttackCommand>>()
                .To<AttackCommandCommandCreator>().AsSingle();
            Container.Bind<CommandCreatorBase<IMoveCommand>>()
                .To<MoveCommandCommandCreator>().AsSingle();
            Container.Bind<CommandCreatorBase<IPatrolCommand>>()
                .To<PatrolCommandCommandCreator>().AsSingle();
            Container.Bind<CommandCreatorBase<IStopCommand>>()
                .To<StopCommandCommandCreator>().AsSingle();
            Container.Bind<CommandCreatorBase<ISetDestinationCommand>>()
                .To<SetDestinationCommandCommandCreator>().AsSingle();
            Container.Bind<CommandCreatorBase<IHealingCommand>>()
                .To<HealingCommandCommandCreator>().AsSingle();

            Container.Bind<CommandButtonsModel>().AsSingle();

            Container.Bind<float>().WithId("Unit").FromInstance(5f);
            Container.Bind<string>().WithId("Unit").FromInstance("Unit");
            Container.Bind<Sprite>().WithId("Unit").FromInstance(_unitSprite);
            
            Container.Bind<QueuePanelModel>().AsSingle();
        }
    }
}