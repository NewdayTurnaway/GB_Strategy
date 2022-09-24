using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using Utils;
using Zenject;

namespace UserControlSystem.CommandsRealization
{
    public class ProduceUnitCommand : IProduceUnitCommand
    {
        [InjectAsset("Unit")] private readonly GameObject _unitPrefab;
        [Inject(Id = "Unit")] public string UnitName { get; }
        [Inject(Id = "Unit")] public Sprite Icon { get; }
        [Inject(Id = "Unit")] public float ProduceTime { get; }

        public GameObject UnitPrefab => _unitPrefab;
    }
}