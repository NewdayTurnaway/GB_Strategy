using UnityEngine;

namespace Abstractions
{
    [System.Serializable]
    public sealed class Health
    {
        [field: SerializeField] public float MaxHealth { get; set; }

        private float _currentHealth;
        
        public float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
        }

        public void SetMaxToCurrent() => 
            CurrentHealth = MaxHealth;
    }
}