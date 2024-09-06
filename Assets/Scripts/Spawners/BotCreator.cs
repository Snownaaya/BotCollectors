using UnityEngine;
using Zenject;

public class BotCreator : MonoBehaviour
{
    [Inject] private DiContainer _container;

    [SerializeField] private StateMachine _botState;

    public StateMachine CreateBot()
    {
        StateMachine bot = _container.InstantiatePrefabForComponent<StateMachine>(_botState);
        return bot;
    }
}