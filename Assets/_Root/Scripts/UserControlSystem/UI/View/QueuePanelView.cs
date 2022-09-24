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
        private const string NO_QUEUE = "No Queue";

        [SerializeField] private Slider _progressSlider;
        [SerializeField] private TextMeshProUGUI _currentUnitName;
        [SerializeField] private Image _currentUnitIcon;

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
            for (int i = 0; i < _buttons.Length; i++)
            {
                _buttons[i].image.sprite = null;
                _buttons[i].gameObject.SetActive(false);
            }
            _progressSlider.gameObject.SetActive(false);
            _currentUnitIcon.sprite = null;
            _currentUnitIcon.enabled = false;
            _currentUnitName.text = NO_QUEUE;
            _unitProductionTaskCt?.Dispose();
        }

        public void SetTask(IUnitProductionTask task, int index)
        {
            _buttons[index].gameObject.SetActive(true);
            _buttons[index].image.sprite = task.Icon;

            if (index == 0)
            {
                _progressSlider.gameObject.SetActive(true);
                _currentUnitIcon.enabled = true;
                _currentUnitIcon.sprite = task.Icon;
                _currentUnitName.text = task.UnitName;
                _unitProductionTaskCt = Observable.EveryUpdate()
                    .Subscribe(_ => _progressSlider.value = task.TimeLeft / task.ProduceTime);
            }
        }

        public void RemoveTask(int index)
        {
            _buttons[index].gameObject.SetActive(false);
            _buttons[index].image.sprite = null;

            if (index == 0)
            {
                _progressSlider.gameObject.SetActive(false);
                _currentUnitIcon.sprite = null;
                _currentUnitIcon.enabled = false;
                _currentUnitName.text = NO_QUEUE;
                _unitProductionTaskCt?.Dispose();
            }
        }
    }
}