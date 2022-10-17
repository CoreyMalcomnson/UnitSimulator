using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SellTask : BaseTask
{
    public event Action OnDelivering;

    [SerializeField] private float gatherDelay = 1f;
    [SerializeField] private float deliverDelay = 1f;

    private enum State
    {
        NavigatingToStockpile,
        WaitingForResources,
        GatheringResources, // Repeat start
        DeliveringResources, // Repeat end
    }

    private Unit unit;
    private Mover mover;
    private ResourceInventory resourceInventory;
    private float timer;
    private State state;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void Start()
    {
        mover = unit.GetMover();
        resourceInventory = unit.GetResourceInventory();
    }

    private void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;

        // Update
        switch (state)
        {
            case State.NavigatingToStockpile:
                if (!mover.IsNavigating())
                {
                    SwitchState(State.GatheringResources);
                }
                break;
            case State.WaitingForResources:
                if (!Stockpile.Instance.GetResourceInventory().IsEmpty())
                {
                    SwitchState(State.GatheringResources);
                }
                break;
            case State.GatheringResources:
                if (timer <= 0 && !mover.IsNavigating())
                {
                    ResourceInventory stockpileResourceInventory = Stockpile.Instance.GetResourceInventory();
                    stockpileResourceInventory.TryGiveOneResource(resourceInventory);

                    if (stockpileResourceInventory.IsEmpty() || resourceInventory.IsFull())
                    {
                        SwitchState(State.DeliveringResources);
                        break;
                    }

                    mover.MoveTo(Stockpile.Instance.GetRandomPoint());
                    timer = gatherDelay;
                }

                break;
            case State.DeliveringResources:
                if (Spaceship.Instance.GetState() != Spaceship.State.Docked)
                    timer = deliverDelay;

                if (timer <= 0)
                {
                    resourceInventory.TryGiveOneResource(Spaceship.Instance.GetResourceInventory());
                    
                    OnDelivering?.Invoke();

                    if (resourceInventory.IsEmpty())
                    {
                        SwitchState(State.WaitingForResources);
                    }

                    timer = deliverDelay;
                }

                break;
        }
    }

    private void SwitchState(State newState)
    {
        // Exit
        switch (state)
        {
            case State.NavigatingToStockpile:
                break;
            case State.GatheringResources:
                break;
            case State.DeliveringResources:
                break;
        }

        state = newState;

        // Enter
        switch (state)
        {
            case State.NavigatingToStockpile:
                mover.MoveTo(Stockpile.Instance.GetPosition());
                break;
            case State.GatheringResources:
                mover.MoveTo(Stockpile.Instance.GetRandomPoint());
                timer = gatherDelay;
                break;
            case State.DeliveringResources:
                timer = deliverDelay;
                break;
        }
    }

    public override void StartTask(TaskArgs e, Action onTaskComplete)
    {
        base.StartTask(e, onTaskComplete);
        SwitchState(State.NavigatingToStockpile);
    }

    public override void TaskComplete()
    {
        mover.Stop();
        base.TaskComplete();
    }

    public override void CancelTask()
    {
        switch (state)
        {
            case State.NavigatingToStockpile:
                break;
            case State.GatheringResources:
                break;
            case State.DeliveringResources:
                break;
        }

        if (!resourceInventory.IsEmpty())
        {
            RetrievableManager.Instance.PlaceRetrievable(transform.position, resourceInventory);
        }

        mover.Stop();
        base.CancelTask();
    }
}
