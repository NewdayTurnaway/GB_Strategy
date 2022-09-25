using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public sealed class FactionManager
    {
        private readonly Dictionary<int, List<int>> _membersCount = new();

        public int FactionsCount
        {
            get
            {
                lock (_membersCount)
                {
                    return _membersCount.Count;
                }
            }
        }

        public int GetWinner()
        {
            lock (_membersCount)
            {
                return _membersCount.Keys.First();
            }
        }

        public void Register(int factionId, int instanceId)
        {
            lock (_membersCount)
            {
                if (!_membersCount.ContainsKey(factionId))
                {
                    _membersCount.Add(factionId, new List<int>());
                }
                if (!_membersCount[factionId].Contains(instanceId))
                {
                    _membersCount[factionId].Add(instanceId);
                }
            }
        }

        public void Unregister(int factionId, int instanceId)
        {
            lock (_membersCount)
            {
                if (_membersCount[factionId].Contains(instanceId))
                {
                    _membersCount[factionId].Remove(instanceId);
                }
                if (_membersCount[factionId].Count == 0)
                {
                    _membersCount.Remove(factionId);
                }
            }
        }
    }
}