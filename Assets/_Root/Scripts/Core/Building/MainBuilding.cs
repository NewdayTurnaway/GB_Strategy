using Abstractions;
using UnityEngine;

namespace Core
{
    public sealed class MainBuilding : MonoBehaviour, IAttackable, IDamageDealer
    {
        [Header("Build Settings")]
        [SerializeField] private Transform _pivotPoint;
        [SerializeField] private Health _health;
        [SerializeField] private float _startHealth;
        [SerializeField] private Sprite _icon;

        [Header("Unit Settings")]
        [SerializeField] private int _unitDamage = 20;

        public Transform PivotPoint => _pivotPoint;
        public Health Health => _health;
        public Sprite Icon => _icon;
        public Vector3 Destination { get; set; }

        public int Damage => _unitDamage;

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