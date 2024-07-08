using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(BotIdleState), typeof(MovementState), typeof(MoveToBaseState))]
[RequireComponent(typeof(CollectResourceState))]
public class StateMachine : MonoBehaviour
{
    [SerializeField] private Bot _bot;
    [SerializeField] private BaseScanner _scan;
    [SerializeField] private Base _base;

    private Dictionary<Type, BotState> _states = new Dictionary<Type, BotState>();

    private Resource _currentResource;
    private BotState _currentState;

    private bool _isBusyBot = false;
    public bool IsBusyBot => _isBusyBot;

    public Type CurrentState => _currentState.GetType();

    private void Awake()
    {
        _states.Add(typeof(BotIdleState), GetComponent<BotIdleState>());
        _states.Add(typeof(MovementState), GetComponent<MovementState>());
        _states.Add(typeof(CollectResourceState), GetComponent<CollectResourceState>());
        _states.Add(typeof(MoveToBaseState), GetComponent<MoveToBaseState>());
    }

    private void Start() =>
        ChangeState(typeof(BotIdleState));

    public void SetBotAsBusy() =>
        _isBusyBot = true;

    public void SetBotAsFree() =>
        _isBusyBot = false;

    public void ChangeState(Type type)
    {
        _currentState?.Exit();
        _currentState = _states[type];

        _currentState.Init(_currentResource, _base, this);
        _currentState.Enter();
        _scan.ScanForResources();
    }

    public void StartMove(Resource resource)
    {
        _currentResource = resource;
        ChangeState(typeof(MovementState));
    }

    public void StartCollect()
    {
        if (Vector3.Distance(_bot.transform.position, _currentResource.transform.position) < 1f)
            ChangeState(typeof(CollectResourceState));
    }

    public void MoveToBase()
    {
        ChangeState(typeof(MoveToBaseState));
    }

    public void FinishWork()
    {
        _currentResource.Release();
        _base.ResourceCollected(_currentResource);
        ChangeState(typeof(BotIdleState));
    }
}