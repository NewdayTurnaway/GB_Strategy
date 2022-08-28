using Abstractions;
using UnityEngine;

namespace Core
{
    public sealed class Unit : MonoBehaviour, ISelectable
    {
        [Header("Unit Settings")]
        [SerializeField] private Health _health;
        [SerializeField] private float _startHealth;
        [SerializeField] private Sprite _icon;
        
        public Health Health => _health;
        public Sprite Icon => _icon;

        private void Awake()
        {
            _health.SetMaxToCurrent();
            _health.CurrentHealth = _startHealth;
        }
    }
}