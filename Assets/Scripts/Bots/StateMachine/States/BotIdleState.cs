public class BotIdleState : BotState
{
    public override void Enter()
    {
        base.Enter();
        CurrentBotState.SetDestination(Base.transform.position);
    }

    public override void Exit() =>
        base.Exit();
}