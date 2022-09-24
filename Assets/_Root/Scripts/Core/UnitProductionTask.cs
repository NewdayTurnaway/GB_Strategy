using Abstractions;
using UnityEngine;

namespace Core
{
    public class UnitProductionTask : IUnitProductionTask
    {
        public GameObject UnitPrefab { get; }
        public string UnitName { get; }
        public Sprite Icon { get; }
        public float ProduceTime { get; }
        public float TimeLeft { get; set; }

        public UnitProductionTask(GameObject unitPrefab, string unitName, Sprite icon, float produceTime)
        {
            UnitPrefab = unitPrefab;
            UnitName = unitName;
            Icon = icon;
            ProduceTime = produceTime;
            TimeLeft = produceTime;
        }
    }
}