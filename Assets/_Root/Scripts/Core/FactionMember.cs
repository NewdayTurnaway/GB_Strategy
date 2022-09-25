using Abstractions;
using UnityEngine;

namespace Core
{
    public class FactionMember : MonoBehaviour, IFactionMember
    {
        [SerializeField] private int _factionId;
        
        public int FactionId => _factionId;

        public void SetFaction(int factionId)
        {
            _factionId = factionId;
        }
    }
}