using Abstractions;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Animator), typeof(StopCommandExecutor))]
    public sealed class Unit : MonoBehaviour, IAttackable, IUnit, IDamageDealer
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private StopCommandExecutor _stopCommand;

        [Header("Unit Settings")]
        [SerializeField] private Transform _pivotPoint;
        [SerializeField] private Health _health;
        [SerializeField] private float _startHealth;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _damage = 20;

        private static readonly int _deathAnimation = Animator.StringToHash("Death");

        public Transform PivotPoint => _pivotPoint;
        public Health Health => _health;
        public Sprite Icon => _icon;
        public int Damage => _damage;

        private void OnValidate()
        {
            _pivotPoint ??= transform;
            _animator ??= GetComponent<Animator>();
            _stopCommand ??= GetComponent<StopCommandExecutor>();
        }

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
                _animator.SetTrigger(_deathAnimation);
                Destroy(gameObject, 1f);
            }
        }

        private async void OnDestroy() => 
            await _stopCommand.ExecuteSpecificCommand(new StopCommand());
    }
}