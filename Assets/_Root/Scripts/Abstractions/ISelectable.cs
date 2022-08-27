using UnityEngine;

namespace Abstractions
{
    public interface ISelectable
    {
        Health Health { get; }
        Sprite Icon { get; }
    }
}