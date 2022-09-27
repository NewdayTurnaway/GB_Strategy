using System.Collections.Concurrent;
using System.Threading.Tasks;
using Abstractions.Commands.CommandsInterfaces;
using UniRx;
using UnityEngine;

namespace Core
{
    public sealed class AutoAttackEvaluator : MonoBehaviour
    {
        //TODO: MessageBroker
        public static ConcurrentDictionary<GameObject, AttackerParallelnfo> AttackersInfo = new();
        public static ConcurrentDictionary<GameObject, FactionMemberParallelInfo> FactionMembersInfo = new();
        public static Subject<Command> AutoAttackCommands = new();

        private void Update() => 
            Parallel.ForEach(AttackersInfo, kvp => Evaluate(kvp.Key, kvp.Value));

        private void Evaluate(GameObject go, AttackerParallelnfo info)
        {
            if (info.CurrentCommand is IMoveCommand)
            {
                return;
            }

            if (info.CurrentCommand is IAttackCommand && info.CurrentCommand is not Command)
            {
                return;
            }

            var factionInfo = default(FactionMemberParallelInfo);
            if (!FactionMembersInfo.TryGetValue(go, out factionInfo))
            {
                return;
            }

            foreach (var (otherGo, otherFactionInfo) in FactionMembersInfo)
            {
                if (factionInfo.Faction == otherFactionInfo.Faction)
                {
                    continue;
                }

                var distance = Vector3.Distance(factionInfo.Position, otherFactionInfo.Position);
                if (distance > info.VisionRadius)
                {
                    continue;
                }

                AutoAttackCommands.OnNext(new Command(go, otherGo));
                break;
            }
        }
    }
}