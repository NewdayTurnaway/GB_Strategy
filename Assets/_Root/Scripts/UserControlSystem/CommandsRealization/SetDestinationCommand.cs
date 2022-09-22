using Abstractions;
using UnityEngine;

namespace UserControlSystem.CommandsRealization
{
    public sealed class SetDestinationCommand : ISetDestinationCommand
    {
        public Vector3 Destination { get; }
        
        public SetDestinationCommand(Vector3 destination)
        {
            Destination = destination;
        }
    }
}