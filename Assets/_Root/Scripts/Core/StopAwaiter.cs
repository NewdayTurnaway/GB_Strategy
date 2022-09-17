using System;
using Utils;

namespace Core
{
    public class StopAwaiter : IAwaiter<AsyncExtensions.Void>
    {
        private readonly UnitMovementStop _unitMovementStop;
        private Action _continuation;
        private bool _isCompleted;

        public bool IsCompleted => _isCompleted;

        public StopAwaiter(UnitMovementStop unitMovementStop)
        {
            _unitMovementStop = unitMovementStop;
            _unitMovementStop.OnStop += OnStop;
        }

        public AsyncExtensions.Void GetResult() =>
            new();

        public void OnCompleted(Action continuation)
        {
            if (_isCompleted)
            {
                continuation?.Invoke();
            }
            else
            {
                _continuation = continuation;
            }
        }

        private void OnStop()
        {
            _unitMovementStop.OnStop -= OnStop;
            _isCompleted = true;
            _continuation?.Invoke();
        }
    }
}