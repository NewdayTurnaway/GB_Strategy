using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;

namespace UserControlSystem.CommandsRealization
{
    public sealed class TeleportCommand : ITeleportCommand
    {
        public Vector3 Target { get; }
        
        public TeleportCommand(Vector3 target)
        {
            Target = target;
        }
    }
}