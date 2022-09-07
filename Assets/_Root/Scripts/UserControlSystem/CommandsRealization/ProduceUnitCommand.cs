﻿using Abstractions.Commands.CommandsInterfaces;
using UnityEngine;
using Utils;

namespace UserControlSystem.CommandsRealization
{
    public class ProduceUnitCommand : IProduceUnitCommand
    {
        [InjectAsset("Unit")] private readonly GameObject _unitPrefab;

        public GameObject UnitPrefab => _unitPrefab;
    }
}