public class BaseConstructionState : BotState
{
    public override void Enter()
    {
        base.Enter();
        CurrentBotState.SetDestination(Flag.transform.position);
        CurrentBotState.DestinationReached += StateMachine.CompleteBaseConstruction;
    }

    public override void Exit()
    {
        base.Exit();
        CurrentBotState.DestinationReached -= StateMachine.CompleteBaseConstruction;
    }
}