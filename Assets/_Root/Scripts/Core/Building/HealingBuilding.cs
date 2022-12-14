using Abstractions;
using UnityEngine;

namespace Core
{
    public sealed class HealingBuilding : MonoBehaviour, IAttackable
    {
        [Header("Build Settings")]
        [SerializeField] private Transform _pivotPoint;
        [SerializeField] private Health _health;
        [SerializeField] private float _startHealth;
        [SerializeField] private Sprite _icon;

        public Transform PivotPoint => _pivotPoint;
        public Health Health => _health;
        public Sprite Icon => _icon;

        private void OnValidate() => 
            _pivotPoint ??= transform;

        private void Awake()
        {
            _health.SetMaxToCurrent();
            _health.CurrentHealth = _startHealth;
        }

        public void RecieveDamage(int amount)
        {
            if (_health.CurrentHealth <= 0)
            {
                return;
            }

            _health.CurrentHealth -= amount;

            if (_health.CurrentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}