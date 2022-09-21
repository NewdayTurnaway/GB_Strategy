using System;
using Abstractions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core
{
    public sealed class TimeModel : ITimeModel, ITickable
    {
        private readonly ReactiveProperty<float> _gameTime = new();

        public IObservable<int> GameTime => _gameTime.Select(f => (int)f);

        public void Tick()
        {
            _gameTime.Value += Time.deltaTime;
        }
    }
}