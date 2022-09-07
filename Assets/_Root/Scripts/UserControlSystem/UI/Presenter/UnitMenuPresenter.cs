using Abstractions;
using UnityEngine;

namespace UserControlSystem
{
    public sealed class UnitMenuPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject _unitMenu;
        [SerializeField] private SelectableValue _selectedValue;

        private void OnValidate() => 
            _unitMenu ??= gameObject;

        private void Start()
        {
            _selectedValue.OnSelected += ONSelected;
            ONSelected(_selectedValue.CurrentValue);
        }
        
        private void ONSelected(ISelectable selected)
        {
            _unitMenu.SetActive(false);

            if (selected != null)
                _unitMenu.SetActive(true);
        }
    }
}