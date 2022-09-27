using UnityEngine;

namespace Core
{
    public struct FactionMemberParallelInfo
    {
        public Vector3 Position;
        public int Faction;

        public FactionMemberParallelInfo(Vector3 position, int faction)
        {
            Position = position;
            Faction = faction;
        }
    }
}