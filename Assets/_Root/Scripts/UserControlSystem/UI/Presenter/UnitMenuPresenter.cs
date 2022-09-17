using Abstractions;
using UnityEngine;
using Zenject;

namespace UserControlSystem
{
    public sealed class UnitMenuPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject _unitMenu;

        [Inject] private readonly SelectableValue _selectedValue;

        private void OnValidate() => 
            _unitMenu ??= gameObject;

        private void Start()
        {
            _selectedValue.OnNewValue += OnSelected;
            OnSelected(_selectedValue.CurrentValue);
        }
        
        private void OnSelected(ISelectable selected)
        {
            _unitMenu.SetActive(false);

            if (selected != null)
            {
                _unitMenu.SetActive(true);
            }
        }
    }
}