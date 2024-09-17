public class MovementState : BotState
{
    public override void Enter()
    {
        base.Enter();
        CurrentBotState.SetDestination(Resource.transform.position);
        CurrentBotState.DestinationReached += StateMachine.StartCollect;
    }

    public override void Exit()
    {
        base.Exit();
        CurrentBotState.DestinationReached -= StateMachine.StartCollect;
    }
}