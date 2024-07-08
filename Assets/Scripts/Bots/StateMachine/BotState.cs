using UnityEngine;

public abstract class BotState : MonoBehaviour
{
    [field: SerializeField] public Bot CurrentBotState { get; private set; }
    [field: SerializeField] public ResourcePool ResourcePool { get; private set; }

    public Base Base { get; private set; }
    public Resource Resource { get; private set; }
    public StateMachine StateMachine { get; private set; }

    private void Awake() =>
        enabled = false;

    public void Init(Resource resource, Base @base, StateMachine state)
    {
        Resource = resource;
        Base = @base;
        StateMachine = state;
    }

    public virtual void Enter() =>
        enabled = true;

    public virtual void Exit() =>
        enabled = false;
}