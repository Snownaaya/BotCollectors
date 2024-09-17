using Zenject;

public class BotInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<StateMachine>().FromNewComponentOnNewGameObject().AsTransient();
    }
}
