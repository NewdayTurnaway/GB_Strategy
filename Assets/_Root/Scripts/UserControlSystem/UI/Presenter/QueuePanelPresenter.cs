using System;
using UnityEngine;
using UserControlSystem.UI.View;
using Zenject;
using UniRx;

namespace UserControlSystem.UI.Presenter
{
    public sealed class QueuePanelPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject _queuePanel;

        private IDisposable _productionQueueAddCt;
        private IDisposable _productionQueueRemoveCt;
        private IDisposable _productionQueueReplaceCt;
        private IDisposable _cancelButtonCts;

        private void OnValidate() =>
            _queuePanel ??= gameObject;

        [Inject]
        private void Init(QueuePanelModel model, QueuePanelView view)
        {
            model.UnitProducers.Subscribe(unitProducer =>
            {
                _productionQueueAddCt?.Dispose();
                _productionQueueRemoveCt?.Dispose();
                _productionQueueReplaceCt?.Dispose();
                _cancelButtonCts?.Dispose();
	
                view.Clear();

                bool unitProducerExist = unitProducer != null;
                _queuePanel.SetActive(unitProducerExist);
	
                if (unitProducerExist)
                {
                    _productionQueueAddCt = unitProducer.Queue
                        .ObserveAdd()
                        .Subscribe(addEvent => view.SetTask(addEvent.Value, addEvent.Index));

                    _productionQueueRemoveCt = unitProducer.Queue
                        .ObserveRemove()
                        .Subscribe(removeEvent => view.RemoveTask(removeEvent.Index));

                    _productionQueueReplaceCt = unitProducer.Queue
                        .ObserveReplace()
                        .Subscribe(replaceEvent => view.SetTask(replaceEvent.NewValue, replaceEvent.Index));

                    _cancelButtonCts = view.CancelButtonClicks.Subscribe(unitProducer.Cancel);

                    for (int i = 0; i < unitProducer.Queue.Count; i++)
                    {
                        view.SetTask(unitProducer.Queue[i], i);
                    }
                }
            });
        }
    }
}