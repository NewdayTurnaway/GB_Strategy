using System.Linq;
using Abstractions;
using UnityEngine;
using UserControlSystem;

public sealed class MouseInteractionPresenter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private SelectableValue _selectedObject;

    private void OnValidate() => 
        _camera ??= Camera.main;

    private void Update()
    {
        if (!Input.GetMouseButtonUp(0))
            return;

        RaycastHit[] hits = Physics.RaycastAll(_camera.ScreenPointToRay(Input.mousePosition));
        if (hits.Length == 0)
            return;

        ISelectable selectable = hits
            .Select(hit => hit.collider.GetComponentInParent<ISelectable>())
            .FirstOrDefault(c => c != null);

        _selectedObject.SetValue(selectable);
    }
}