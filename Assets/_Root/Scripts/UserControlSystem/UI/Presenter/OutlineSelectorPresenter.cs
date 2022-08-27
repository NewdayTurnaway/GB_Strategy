using Abstractions;
using UnityEngine;

namespace UserControlSystem
{
    public sealed class OutlineSelectorPresenter : MonoBehaviour
    {
        [SerializeField] private SelectableValue _selectedValue;

        private OutlineSelector[] _outlineSelectors;

        private void Start()
        {
            _selectedValue.OnSelected += ONSelected;
            ONSelected(_selectedValue.CurrentValue);
        }

        private void ONSelected(ISelectable selected)
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
                return;

            for (int i = 0; i < selectors.Length; i++)
            {
                selectors[i].SetSelected(value);
            }
        }
    }
}