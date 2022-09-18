using System;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class UnitMovementStop : MonoBehaviour, IAwaitable<AsyncExtensions.Void>
    {
        [SerializeField] private NavMeshAgent _agent;
        
        public event Action OnStop;

        private void OnValidate() => 
            _agent ??= GetComponent<NavMeshAgent>();


        private void Update()
        {
            if (_agent.pathPending)
            {
                return;
            }
            if (_agent.remainingDistance > _agent.stoppingDistance)
            {
                return;
            }
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
            {
                OnStop?.Invoke();
            }
        }

        public IAwaiter<AsyncExtensions.Void> GetAwaiter() =>
            new StopAwaiter(this);
    }
}