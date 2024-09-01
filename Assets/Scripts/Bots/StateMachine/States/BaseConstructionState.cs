public class BaseConstructionState : BotState
{
    public override void Enter()
    {
        base.Enter();
        CurrentBotState.SetDestination(Flag.transform.position);
        CurrentBotState.OnDestinationReached += HandleConstruction;
    }

    public override void Exit()
    {
        base.Exit();
        CurrentBotState.OnDestinationReached -= HandleConstruction;
    }

    private void HandleConstruction()
    {
        StateMachine.CompleteBaseConstruction();
    }
}