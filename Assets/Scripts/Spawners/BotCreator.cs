using UnityEngine;

public class BotCreator : MonoBehaviour
{
    [SerializeField] private StateMachine _botState;
    [SerializeField] private Base _base;
    [SerializeField] private ResourceScanner _resourceScanner;

    public StateMachine CreateBot()
    {
        StateMachine newBot = Instantiate(_botState);
        newBot.Init(_base, _resourceScanner);
        return newBot;
    }
}