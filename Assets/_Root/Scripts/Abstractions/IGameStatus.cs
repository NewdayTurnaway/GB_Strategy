using System;

namespace Abstractions
{
    public interface IGameStatus
    {
        IObservable<int> Status { get; }
    }
}