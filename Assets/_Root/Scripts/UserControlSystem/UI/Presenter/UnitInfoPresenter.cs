using Abstractions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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

        [Inject] private readonly SelectableValue _selectedValue;

        private void OnValidate() => 
            _rectTransform ??= (RectTransform)gameObject.transform;

        private void Start() => 
            _selectedValue.Subscribe(OnSelected);

        private void OnSelected(ISelectable selected)
        {
            _selectedImage.enabled = selected != null;
            _healthSlider.gameObject.SetActive(selected != null);
            _text.enabled = selected != null;

            if (selected != null)
            {
                Observable.EveryFixedUpdate().Subscribe(_ => SetUiInfo(selected));
                LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
            }
        }

        private void SetUiInfo(ISelectable selected)
        {
            _selectedImage.sprite = selected.Icon;

            _text.text = $"{selected.Health.CurrentHealth} / {selected.Health.MaxHealth}";

            _healthSlider.minValue = 0;
            _healthSlider.maxValue = selected.Health.MaxHealth;
            _healthSlider.value = selected.Health.CurrentHealth;

            var color = Color.Lerp(_minHealthColor, _maxHealthColor, selected.Health.CurrentHealth / selected.Health.MaxHealth);
            _sliderFillImage.color = color;
        }
    }
}