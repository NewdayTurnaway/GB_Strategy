using System.Collections.Generic;
using UnityEngine;

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
        if (_isSelected == isSelected)
            return;

        List<Material> materialsList = new(_renderer.materials);
        
        if (isSelected)
            materialsList.Add(_outlineMaterial);
        else
            materialsList.RemoveAt(materialsList.Count - 1);

        _renderer.materials = materialsList.ToArray();

        _isSelected = isSelected;
    }
}
