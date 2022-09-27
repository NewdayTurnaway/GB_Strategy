using Abstractions.Commands;

namespace Core
{
    public struct AttackerParallelnfo
    {
        public float VisionRadius;
        public ICommand CurrentCommand;

        public AttackerParallelnfo(float visionRadius, ICommand currentCommand)
        {
            VisionRadius = visionRadius;
            CurrentCommand = currentCommand;
        }
    }
}