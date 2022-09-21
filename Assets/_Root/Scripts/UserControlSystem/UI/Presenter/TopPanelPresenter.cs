using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using System;
using Abstractions;

namespace UserControlSystem
{
    public sealed class TopPanelPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _menuButton;
        [SerializeField] private RectTransform _gameMenuRectTransform;

        [Inject]
        private void Init(ITimeModel timeModel)
        {
            timeModel.GameTime.Subscribe(seconds =>
            {
                var t = TimeSpan.FromSeconds(seconds);
                _inputField.text = $"{t.Minutes:D2}:{t.Seconds:D2}";
            });

            _menuButton.OnClickAsObservable().Subscribe(_ => _gameMenuRectTransform.gameObject.SetActive(true));
        }
    } 
}