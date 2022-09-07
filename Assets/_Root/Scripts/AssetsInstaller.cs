using UnityEngine;
using UserControlSystem;
using Utils;
using Zenject;

[CreateAssetMenu(fileName = "AssetsInstaller", menuName = "Installers/AssetsInstaller")]
public class AssetsInstaller : ScriptableObjectInstaller<AssetsInstaller>
{
    [SerializeField] private AssetsContext _legacyContext;
    
    private readonly SelectableValue _selectableValue = new();
    private readonly Vector3Value _vector3Value = new();
    private readonly AttackableValue _attackableValue = new();

    public override void InstallBindings()
    {
        Container.BindInstances(_legacyContext, _selectableValue, _vector3Value, _attackableValue);
    }
}