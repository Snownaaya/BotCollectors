using UnityEngine;
using Zenject;

public class ResourcePoolInstaller : MonoInstaller
{
    [SerializeField] private ResourcePool _resourcePool;

    public override void InstallBindings()
    {
        Container.Bind<ResourcePool>().FromInstance(_resourcePool).AsSingle();
    }
}