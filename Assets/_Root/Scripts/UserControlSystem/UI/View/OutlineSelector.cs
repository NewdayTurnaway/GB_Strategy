using System.Collections.Generic;
using UnityEngine;

namespace UserControlSystem.UI.View
{
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class OutlineSelector : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Material _outlineMaterial;

        private bool _isSelected;

        private void OnValidate() =>
            _renderer ??= GetComponent<MeshRenderer>();

        public void SetSelected(bool isSelected)
        {
            if (this == null)
            {
                return;
            }

            if (_isSelected == isSelected)
            {
                return;
            }

            var materialsList = new List<Material>(_renderer.materials);

            if (isSelected)
            {
                materialsList.Add(_outlineMaterial);
            }
            else
            {
                materialsList.RemoveAt(materialsList.Count - 1);
            }

            _renderer.materials = materialsList.ToArray();

            _isSelected = isSelected;
        }
    }
}
