using Abstractions.Commands;
using UnityEngine;

namespace Abstractions
{
    public interface ISetDestinationCommand : ICommand
    {
        Vector3 Destination { get; }
    }
}