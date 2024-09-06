public class MoveToBaseState : BotState
{
    public override void Enter()
    {
        base.Enter();
        CurrentBotState.SetDestination(Base.transform.position);
        CurrentBotState.OnDestinationReached += StateMachine.FinishWork;
    }

    public override void Exit()
    {
        base.Exit();
        CurrentBotState.OnDestinationReached -= StateMachine.FinishWork;
    }
}