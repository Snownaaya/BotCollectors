using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class Bot : MonoBehaviour
{
    [SerializeField] private BaseCreator _baseCreator;

    private NavMeshAgent _agent;

    public event Action OnDestinationReached;

    private void Awake() =>
        _agent = GetComponent<NavMeshAgent>();

    private void Start()
    {
        if (_agent != null)
            _agent.stoppingDistance = 1f;
    }

    private void FixedUpdate()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
            OnDestinationReached?.Invoke();
    }

    public void SetDestination(Vector3 destination) =>
        _agent.SetDestination(destination);

    public void BuildNewBase(StateMachine transfferingBot, Flag currentFlag, Vector3 position)
    {
        if (currentFlag == null) return;

        _baseCreator.CreateBase(currentFlag.transform.position, transfferingBot);
        currentFlag.TurnOff();
    }
}