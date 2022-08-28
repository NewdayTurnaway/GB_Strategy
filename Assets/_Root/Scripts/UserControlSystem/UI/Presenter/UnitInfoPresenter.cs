﻿using Abstractions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserControlSystem
{
    public sealed class UnitInfoPresenter : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private Image _selectedImage;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _sliderBackground;
        [SerializeField] private Image _sliderFillImage;
        [SerializeField] private Color _maxHealthColor;
        [SerializeField] private Color _minHealthColor;

        [SerializeField] private SelectableValue _selectedValue;

        private void OnValidate() => 
            _rectTransform ??= (RectTransform)gameObject.transform;

        private void Start()
        {
            _selectedValue.OnSelected += ONSelected;
            ONSelected(_selectedValue.CurrentValue);
        }
        
        private void ONSelected(ISelectable selected)
        {
            _selectedImage.enabled = selected != null;
            _healthSlider.gameObject.SetActive(selected != null);
            _text.enabled = selected != null;

            if (selected != null)
            {
                _selectedImage.sprite = selected.Icon;
                
                _text.text = $"{selected.Health.CurrentHealth} / {selected.Health.MaxHealth}";
                
                _healthSlider.minValue = 0;
                _healthSlider.maxValue = selected.Health.MaxHealth;
                _healthSlider.value = selected.Health.CurrentHealth;
                
                Color color = Color.Lerp(_minHealthColor, _maxHealthColor, selected.Health.CurrentHealth / selected.Health.MaxHealth);
                _sliderFillImage.color = color;

                LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
            }
        }
    }
}