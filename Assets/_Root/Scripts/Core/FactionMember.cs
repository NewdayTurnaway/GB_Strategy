using Abstractions;
using UnityEngine;
using Zenject;

namespace Core
{
    public class FactionMember : MonoBehaviour, IFactionMember
    {
        [SerializeField] private int _factionId;

        [Inject] private FactionManager _factionManager;

        public int FactionId => _factionId;

        private void Awake()
        {
            if (_factionId != 0)
            {
                _factionManager.Register(_factionId, GetInstanceID());
            }
        }

        public void SetFaction(int factionId)
        {
            _factionManager.Unregister(_factionId, GetInstanceID());
            _factionId = factionId;
            _factionManager.Register(_factionId, GetInstanceID());
        }

        private void OnDestroy() => 
            _factionManager.Unregister(_factionId, GetInstanceID());
    }
}