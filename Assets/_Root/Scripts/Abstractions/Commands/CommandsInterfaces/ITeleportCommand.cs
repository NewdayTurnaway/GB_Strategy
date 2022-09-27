using UnityEngine;

namespace Abstractions.Commands.CommandsInterfaces
{
    public interface ITeleportCommand : ICommand
    {
        Vector3 Target { get; }
    }
}