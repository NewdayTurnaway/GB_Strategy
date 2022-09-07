using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using Utils;
using Zenject;

namespace UserControlSystem
{
    public class UIModelInstaller : MonoInstaller
    {
        [SerializeField] private AssetsContext _legacyContext;
        [SerializeField] private SelectableValue _selectableValue;
        [SerializeField] private Vector3Value _vector3Value;
        [SerializeField] private AttackableValue _attackableValue;

        public override void InstallBindings()
        {
            Container.Bind<AssetsContext>().FromInstance(_legacyContext);
            Container.Bind<SelectableValue>().FromInstance(_selectableValue);
            Container.Bind<Vector3Value>().FromInstance(_vector3Value);
            Container.Bind<AttackableValue>().FromInstance(_attackableValue);

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

            Container.Bind<CommandButtonsModel>().AsSingle();
        }
    }
}