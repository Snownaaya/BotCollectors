using Zenject;

public class BaseInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Base>().FromNewComponentOnNewGameObject().AsTransient();
    }
}
