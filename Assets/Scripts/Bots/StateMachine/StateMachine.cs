using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(BotIdleState), typeof(MovementState), typeof(MoveToBaseState))]
[RequireComponent(typeof(CollectResourceState)), RequireComponent(typeof(BaseConstructionState))]
public class StateMachine : MonoBehaviour, IStateMachine
{
    [SerializeField] private Base _base;
    [SerializeField] private ResourceScanner _scan;
    [SerializeField] private FlagHandler _flagHandler;

    private Dictionary<Type, BotState> _states = new Dictionary<Type, BotState>();

    private Bot _bot;
    private Resource _currentResource;
    private ResourcePool _resourcePool;
    private BotState _currentState;
    private BaseCreator _baseFactory;
    private Flag _flag;

    private bool _isBusyBot = false;
    private bool _isContructionBase = false;

    public Type CurrentState =>
        _currentState.GetType();

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

    public void Init(Base baseObj, ResourceScanner resourceScanner)
    {
        _base = baseObj ?? throw new ArgumentNullException(nameof(baseObj));
        _scan = resourceScanner ?? throw new ArgumentNullException(nameof(resourceScanner));

        _base.AddBot(this);
        ChangeState(typeof(BotIdleState));
    }

    public bool SetBotAsBusy() =>
        _isBusyBot = true;

    public bool SetBotAsFree() =>
        _isBusyBot = false;

    public void ChangeState(Type type)
    {
        if (_states.TryGetValue(type, out var newState))
        {
            _currentState?.Exit();
            _currentState = newState ?? throw new ArgumentNullException(nameof(newState));

            _currentState.Init(_base, _bot, _currentResource, _resourcePool, this, _flag, _baseFactory, _flagHandler);

            _currentState.Enter();

            _scan.ScanForResources();
        }
    }

    public void StartSearchingForResource(Resource resource)
    {
        SetCurrentResource(resource);
        ChangeState(typeof(MovementState));
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

    public void MoveToBase() =>
        ChangeState(typeof(MoveToBaseState));

    public void FinishWork()
    {
        _base.ResourceCollect(_currentResource, this);

        _currentResource.Release();
        SetBotAsFree();
        ChangeState(typeof(BotIdleState));
    }


    public void CompleteBaseConstruction()
    {
        _base.CompleteConstruction(this);
        _isContructionBase = false;
        ChangeState(typeof(BotIdleState));
    }

    public void StartConstructingBase(Base @base, Flag flag)
    {
        _base = @base;
        _flag = flag;

        ChangeState(typeof(BaseConstructionState));
    }

    public void SetCurrentResource(Resource resource) =>
        _currentResource = resource;
}
