public class MovementState : BotState
{
    public override void Enter()
    {
        base.Enter();
        CurrentBotState.SetDestination(Resource.transform.position);
        CurrentBotState.OnDestinationReached += StateMachine.StartCollect;
    }

    public override void Exit()
    {
        base.Exit();
        CurrentBotState.OnDestinationReached -= StateMachine.StartCollect;
    }
}