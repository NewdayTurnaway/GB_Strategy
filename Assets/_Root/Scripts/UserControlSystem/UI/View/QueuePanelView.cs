using System;
using Abstractions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UserControlSystem.UI.View
{
    public class QueuePanelView : MonoBehaviour
    {
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TextMeshProUGUI _currentUnitName;

        [SerializeField] private Image[] _images;
        [SerializeField] private GameObject[] _imageHolders;
        [SerializeField] private Button[] _buttons;
        
        private readonly Subject<int> _cancelButtonClicks = new();
        private IDisposable _unitProductionTaskCt;

        public IObservable<int> CancelButtonClicks => _cancelButtonClicks;

        [Inject]
        private void Init()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                var index = i;
                _buttons[i].onClick.AddListener(() => _cancelButtonClicks.OnNext(index));
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _images.Length; i++)
            {
                _images[i].sprite = null;
                _imageHolders[i].SetActive(false);
            }
            _progressSlider.gameObject.SetActive(false);
            _currentUnitName.text = string.Empty;
            _currentUnitName.enabled = false;
            _unitProductionTaskCt?.Dispose();
        }

        public void SetTask(IUnitProductionTask task, int index)
        {
            _imageHolders[index].SetActive(true);
            _images[index].sprite = task.Icon;

            if (index == 0)
            {
                _progressSlider.gameObject.SetActive(true);
                _currentUnitName.text = task.UnitName;
                _currentUnitName.enabled = true;
                _unitProductionTaskCt = Observable.EveryUpdate()
                    .Subscribe(_ => _progressSlider.value = task.TimeLeft / task.ProduceTime);
            }
        }

        public void RemoveTask(int index)
        {
            _imageHolders[index].SetActive(false);
            _images[index].sprite = null;

            if (index == 0)
            {
                _progressSlider.gameObject.SetActive(false);
                _currentUnitName.text = string.Empty;
                _currentUnitName.enabled = false;
                _unitProductionTaskCt?.Dispose();
            }
        }
    }
}