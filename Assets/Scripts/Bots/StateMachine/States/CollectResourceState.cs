public class CollectResourceState : BotState
{
    public override void Enter()
    {
        base.Enter();
        Resource.PickUp(CurrentBotState.transform);
        CurrentBotState.DestinationReached += StateMachine.MoveToBase;
    }

    public override void Exit()
    {
        base.Exit();
        CurrentBotState.DestinationReached -= StateMachine.MoveToBase;
    }
}