using System.Threading.Tasks;
using Abstractions;
using Abstractions.Commands.CommandsInterfaces;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public sealed class HealingCommandExecutor : CommandExecutorBase<IHealingCommand>
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Button _healingButton;
        [SerializeField] private float _addHealth = 50f;
        [SerializeField] private float _healingRadius = 3f;
        [SerializeField] private float _healingDelay = 5f;

        private float _timeLeft;
        private bool _enableButton = true;

        private void OnValidate() => 
            _transform = transform;

        private void Start() =>
            Observable.EveryUpdate().Subscribe(_ => OnUpdate());

        public override Task ExecuteSpecificCommand(IHealingCommand command)
        {
            var hitColliders = Physics.OverlapSphere(_transform.position, _healingRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].TryGetComponent<IUnit>(out _)
                    && hitColliders[i].TryGetComponent(out IAttackable attackable))
                {
                    attackable.Health.CurrentHealth += _addHealth;
                }
            }

            _timeLeft = _healingDelay;
            _enableButton = false;
            _healingButton.interactable = _enableButton;
            return Task.CompletedTask;
        }

        private void OnUpdate()
        {
            if (_enableButton)
            {
                return;
            }

            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                _enableButton = true;
                _healingButton.interactable = _enableButton;
            }
        }
    }
}