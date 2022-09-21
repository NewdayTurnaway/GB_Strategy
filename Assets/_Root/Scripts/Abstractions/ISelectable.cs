using UnityEngine;

namespace Abstractions
{
    public interface ISelectable : IHeathHolder, IIconHolder
    {
        Transform PivotPoint { get; }
    }
}