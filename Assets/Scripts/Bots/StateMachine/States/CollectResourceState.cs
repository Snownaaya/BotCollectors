
public class CollectResourceState : BotState
{
    public override void Enter()
    {
        base.Enter();
        Resource?.PickUp(CurrentBotState.transform);
        CurrentBotState.OnDestinationReached += StateMachine.MoveToBase;
    }

    public override void Exit()
    {
        base.Exit();
        CurrentBotState.OnDestinationReached -= StateMachine.MoveToBase;
    }
}