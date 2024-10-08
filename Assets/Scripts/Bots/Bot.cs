using UnityEngine;
using UnityEngine.AI;
using System;
using Zenject;

[RequireComponent(typeof(NavMeshAgent), typeof(StateMachine))]
public class Bot : MonoBehaviour
{
    [Inject] private ResourceSpawner _resourcePool;

    [SerializeField] private BaseCreator _baseCreator;

    private NavMeshAgent _agent;
    private StateMachine _bot;

    public event Action DestinationReached;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _bot = GetComponent<StateMachine>();
    }

    private void Start()
    {
        if (_agent != null)
            _agent.stoppingDistance = 1f;
    }

    private void FixedUpdate()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
            DestinationReached?.Invoke();
    }

    public void SetDestination(Vector3 destination) =>
        _agent.SetDestination(destination);

    public Base BuildNewBase(Flag currentFlag)
    {
        if (currentFlag == null)
            return null;

        Base @base = _baseCreator.CreateBase(currentFlag.transform.position);
        @base.Init(_bot, _resourcePool);
        currentFlag.TurnOff();

        return @base;
    }
}