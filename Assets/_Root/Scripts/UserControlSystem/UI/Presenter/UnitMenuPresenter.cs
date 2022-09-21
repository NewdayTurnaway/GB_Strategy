using Abstractions;
using UniRx;
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

        [Inject]
        private void Init() => 
            _selectedValue.Subscribe(OnSelected);

        private void OnSelected(ISelectable selected) => 
            _unitMenu.SetActive(selected != null);
    }
}