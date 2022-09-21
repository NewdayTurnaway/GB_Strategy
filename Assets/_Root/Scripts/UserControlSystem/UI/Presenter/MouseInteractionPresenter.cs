using System.Linq;
using Abstractions;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UserControlSystem
{
    public sealed class MouseInteractionPresenter : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private Transform _groundTransform;

        [Inject] private readonly SelectableValue _selectedObject;
        [Inject] private readonly Vector3Value _groundClicksRMB;
        [Inject] private readonly AttackableValue _attackablesRMB;

        private Plane _groundPlane;

        private void OnValidate()
        {
            _camera ??= Camera.main;
            _eventSystem ??= FindObjectOfType<EventSystem>();
        }

        [Inject]
        private void Init()
        {
            _groundPlane = new Plane(_groundTransform.up, 0);

            var availableUiFramesStream = Observable.EveryUpdate()
                .Where(_ => !_eventSystem.IsPointerOverGameObject());

            var leftClicksStream = availableUiFramesStream
                .Where(_ => Input.GetMouseButtonDown(0));
            var rightClicksStream = availableUiFramesStream
                .Where(_ => Input.GetMouseButtonDown(1));

            var lmbRays = leftClicksStream
                .Select(_ => _camera.ScreenPointToRay(Input.mousePosition));
            var rmbRays = rightClicksStream
                .Select(_ => _camera.ScreenPointToRay(Input.mousePosition));

            var lmbHitsStream = lmbRays
                .Select(ray => Physics.RaycastAll(ray));
            var rmbHitsStream = rmbRays
                .Select(ray => (ray, Physics.RaycastAll(ray)));

            lmbHitsStream.Subscribe(hits =>
            {
                if (WhereHit<ISelectable>(hits, out var selectable))
                {
                    _selectedObject.SetValue(selectable);
                }
            });

            rmbHitsStream.Subscribe((ray, hits) =>
            {
                if (WhereHit<IAttackable>(hits, out var attackable))
                {
                    _attackablesRMB.SetValue(attackable);
                }
                else if (_groundPlane.Raycast(ray, out var enter))
                {
                    _groundClicksRMB.SetValue(ray.origin + ray.direction * enter);
                }
            });
        }

        private bool WhereHit<T>(RaycastHit[] hits, out T result) where T : class
        {
            result = default;

            if (hits.Length == 0)
            {
                return false;
            }
            result = hits
                .Select(hit => hit.collider.GetComponentInParent<T>())
                .FirstOrDefault(c => c != null);

            return result != default;
        }
    } 
}