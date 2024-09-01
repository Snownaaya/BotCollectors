using UnityEngine;
using Zenject;

public class ResourcePoolInstaller : MonoInstaller
{
    [SerializeField] private ResourcePool _resourcePool;
    [SerializeField] private StateMachine _stateMachine;
    [SerializeField] private Base _base;

    public override void InstallBindings()
    {
        Container.Bind<ResourcePool>().FromInstance(_resourcePool).AsSingle(); 
        Container.Bind<StateMachine>().FromInstance(_stateMachine).AsSingle();
        Container.Bind<Base>().FromInstance(_base).AsSingle();
    }
}
