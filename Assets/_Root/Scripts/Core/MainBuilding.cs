using Abstractions;
using UnityEngine;

namespace Core
{
    public sealed class MainBuilding : MonoBehaviour, IUnitProducer, ISelectable
    {
        [Header("Unit Produce Settings")]
        [SerializeField] private GameObject _unitPrefab;
        [SerializeField] private Transform _unitsParent;

        [Header("Build Settings")]
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

        public void ProduceUnit()
        {
            Instantiate(_unitPrefab,
                new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)),
                Quaternion.identity,
                _unitsParent);
        }
    }
}