using UnityEngine;
using Zenject;

public class ResourcePoolInstaller : MonoInstaller
{
    [SerializeField] private ResourceSpawner _resourcePool;

    public override void InstallBindings()
    {
        Container.Bind<ResourceSpawner>().FromInstance(_resourcePool).AsSingle();
    }
}