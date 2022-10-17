using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    public event Action OnNavigatingStarted;
    public event Action OnNavigatingCompleted;

    private const float DEFAULT_STOPPING_DISTANCE = 0.5f;

    private NavMeshAgent navMeshAgent;
    private bool isNavigating;
    private Vector3 targetPosition;
    private float stoppingDistance;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!isNavigating) return;

        // print($"{navMeshAgent.isOnOffMeshLink} {navMeshAgent.velocity}");
        if (navMeshAgent.isOnOffMeshLink)
        {
            /*
            OffMeshLinkData data = navMeshAgent.currentOffMeshLinkData;
            navMeshAgent.Warp(data.endPos);
            */
            navMeshAgent.CompleteOffMeshLink();
            MoveTo(targetPosition, stoppingDistance);
        }

        if (HasReachedTargetPosition())
        {
            Stop();
        }
    }

    private bool HasReachedTargetPosition()
    {
        return Vector3.Distance(transform.position, targetPosition) <= stoppingDistance;
    }

    public void MoveTo(Vector3 targetPosition, float stoppingDistance = DEFAULT_STOPPING_DISTANCE)
    {
        isNavigating = true;
        this.targetPosition = targetPosition;
        this.stoppingDistance = stoppingDistance;

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(targetPosition);

        OnNavigatingStarted?.Invoke();
    }

    public void Stop()
    {
        isNavigating = false;

        navMeshAgent.isStopped = true;

        OnNavigatingCompleted?.Invoke();
    }

    public bool IsNavigating()
    {
        return isNavigating;
    }
}