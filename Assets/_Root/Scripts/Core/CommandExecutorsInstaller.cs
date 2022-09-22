using Abstractions.Commands;
using Zenject;

namespace Core
{
    public sealed class CommandExecutorsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var executors = gameObject.GetComponentsInChildren<ICommandExecutor>();
            for (int i = 0; i < executors.Length; i++)
            {
                ICommandExecutor executor = executors[i];
                var baseType = executor.GetType().BaseType;
                Container.Bind(baseType).FromInstance(executor);
            }
        }
    }
}