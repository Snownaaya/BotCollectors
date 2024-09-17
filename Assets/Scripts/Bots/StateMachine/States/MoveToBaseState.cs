public class MoveToBaseState : BotState
{
    public override void Enter()
    {
        base.Enter();
        CurrentBotState.SetDestination(Base.transform.position);
        CurrentBotState.DestinationReached += StateMachine.FinishWork;
    }

    public override void Exit()
    {
        base.Exit();
        CurrentBotState.DestinationReached -= StateMachine.FinishWork;
    }
}