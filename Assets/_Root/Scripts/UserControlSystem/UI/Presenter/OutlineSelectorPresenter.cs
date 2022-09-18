using Abstractions;
using UnityEngine;
using UserControlSystem.UI.View;
using Zenject;

namespace UserControlSystem
{
    public sealed class OutlineSelectorPresenter : MonoBehaviour
    {
        [Inject] private readonly SelectableValue _selectedValue;

        private OutlineSelector[] _outlineSelectors;

        private void Start()
        {
            _selectedValue.OnNewValue += OnSelected;
            OnSelected(_selectedValue.CurrentValue);
        }

        private void OnSelected(ISelectable selected)
        {
            SetOutlineSelected(_outlineSelectors, false);
            _outlineSelectors = null;

            if (selected != null)
            {
                _outlineSelectors = (selected as Component).GetComponentsInParent<OutlineSelector>();
                SetOutlineSelected(_outlineSelectors, true);
            }
        }

        private void SetOutlineSelected(OutlineSelector[] selectors, bool value)
        {
            if (selectors == null)
            {
                return;
            }

            for (int i = 0; i < selectors.Length; i++)
            {
                selectors[i].SetSelected(value);
            }
        }
    }
}