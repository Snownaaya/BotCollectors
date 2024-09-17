using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(BotIdleState), typeof(MovementState), typeof(MoveToBaseState))]
[RequireComponent(typeof(CollectResourceState)), RequireComponent(typeof(BaseConstructionState))]
public class StateMachine : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private FlagSpawner _flagHandler;
    [SerializeField] private ResourceStorage _resourceStorage;

    private Dictionary<Type, BotState> _states = new Dictionary<Type, BotState>();

    private Bot _bot;
    private Resource _currentResource;
    private BotState _currentState;
    private BaseCreator _baseFactory;
    private Flag _flag;

    public bool IsIdle => _currentState.GetType() == typeof(BotIdleState);

    private void Awake()
    {
        _bot = GetComponent<Bot>();

        _states.Add(typeof(BotIdleState), GetComponent<BotIdleState>());
        _states.Add(typeof(MovementState), GetComponent<MovementState>());
        _states.Add(typeof(CollectResourceState), GetComponent<CollectResourceState>());
        _states.Add(typeof(MoveToBaseState), GetComponent<MoveToBaseState>());
        _states.Add(typeof(BaseConstructionState), GetComponent<BaseConstructionState>());
    }

    private void Start() =>
        ChangeState(typeof(BotIdleState));

    public void ChangeState(Type type)
    {
        if (_states.TryGetValue(type, out var newState))
        {
            _currentState?.Exit();
            _currentState = newState ?? throw new ArgumentNullException(nameof(newState));
            _currentState.Init(_base, _bot, _currentResource, this, _flag, _baseFactory, _flagHandler);
            _currentState.Enter();
        }
    }

    public void SetHome(Base @base) =>
        _base = @base;

    public void StartMove(Resource resource)
    {
        _currentResource = resource;
        ChangeState(typeof(MovementState));
    }

    public void StartCollect()
    {
        if (_resourceStorage.TryRequestResource(_currentResource))
        {
            if (Vector3.Distance(_bot.transform.position, _currentResource.transform.position) < 1f)
                ChangeState(typeof(CollectResourceState));
        }
        else
        {
            ChangeState(typeof(BotIdleState));
        }
    }

    public void MoveToBase() =>
        ChangeState(typeof(MoveToBaseState));

    public void FinishWork()
    {
        _base.ResourceCollect(_currentResource);
        _currentResource.Release();
        ChangeState(typeof(BotIdleState));
    }

    public void CompleteBaseConstruction()
    {
        _base.CompleteConstruction(this);

        if (_currentResource != null)
            StartMove(_currentResource);
        else
            ChangeState(typeof(BotIdleState));
    }

    public void StartConstructingBase(Base @base, Flag flag)
    {
        _base = @base;
        _flag = flag;

        ChangeState(typeof(BaseConstructionState));
    }
}