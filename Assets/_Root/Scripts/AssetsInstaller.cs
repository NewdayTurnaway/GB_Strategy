using UnityEngine;
using UserControlSystem;
using Utils;
using Zenject;

[CreateAssetMenu(fileName = "AssetsInstaller", menuName = "Installers/AssetsInstaller")]
public class AssetsInstaller : ScriptableObjectInstaller<AssetsInstaller>
{
    [SerializeField] private AssetsContext _legacyContext;
    [SerializeField] private SelectableValue _selectableValue;
    [SerializeField] private Vector3Value _vector3Value;
    [SerializeField] private AttackableValue _attackableValue;

    public override void InstallBindings()
    {
        Container.BindInstances(_legacyContext, _selectableValue, _vector3Value, _attackableValue);
    }
}