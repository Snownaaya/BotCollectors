using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class Bot : MonoBehaviour
{
    private NavMeshAgent _agent;

    public event Action OnDestinationReached;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = 1f;
    }

    private void FixedUpdate()
    { 
        if (_agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
        {
            OnDestinationReached?.Invoke();
        }
    }

    public void SetDestination(Vector3 destination) =>
        _agent.SetDestination(destination);
}