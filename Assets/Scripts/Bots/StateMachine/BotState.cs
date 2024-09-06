using UnityEngine;

public abstract class BotState : MonoBehaviour
{
    public Bot CurrentBotState { get; private set; }
    public Base Base { get; private set; }
    public Resource Resource { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public BaseCreator BaseCreat { get; private set; }
    public Flag Flag { get; private set; }
    public FlagHandler FlagHandler { get; private set; }

    private void Awake() =>
        enabled = false;

    public void Init(Base @base, Bot bot, Resource resource, StateMachine state, Flag flag , BaseCreator baseCreat, FlagHandler flagHandler)
    {
        CurrentBotState = bot;
        Resource = resource;
        Base = @base;
        StateMachine = state; 
        BaseCreat = baseCreat; 
        Flag = flag;
        FlagHandler = flagHandler;
    }

    public virtual void Enter() =>
        enabled = true;

    public virtual void Update() { }

    public virtual void Exit() =>
        enabled = false;
}