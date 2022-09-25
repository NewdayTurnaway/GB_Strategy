using Abstractions;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core
{
    public sealed class GameStatus : MonoBehaviour, IGameStatus
    {
        [Inject] private readonly FactionManager _factionManager;

        private readonly Subject<int> _status = new();

        public IObservable<int> Status => _status;

        private void CheckStatus(object state)
        {
            if (_factionManager.FactionsCount == 0)
            {
                _status.OnNext(0);
            }
            else if (_factionManager.FactionsCount == 1)
            {
                _status.OnNext(_factionManager.GetWinner());
            }
        }

        private void Update() => 
            ThreadPool.QueueUserWorkItem(CheckStatus);
    }
}