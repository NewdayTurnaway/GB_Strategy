using UnityEngine;

namespace Abstractions
{
    public interface ISelectable
    {
        Transform PivotPoint { get; }
        Health Health { get; }
        Sprite Icon { get; }
    }
}