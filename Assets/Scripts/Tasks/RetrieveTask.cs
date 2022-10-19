using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using CodeMonkey;

public class RetrieveTask : BaseTask
{
    public event Action OnHaulingStarted;
    public event Action OnHaulingCompleted;

    private enum State
    {
        SearchingForRetrievable,
        NavigatingToRetrievable,
        HaulingResources
    }

    private Unit unit;
    private Mover mover;
    private ResourceInventory resourceInventory;

    private State state;
    private Retrievable currentRetrievable;

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

        // Update
        switch (state)
        {
            case State.SearchingForRetrievable:
                currentRetrievable = RetrievableManager.Instance.FindClosestRetrievable(transform.position);

                if (currentRetrievable != null)
                {
                    SwitchState(State.NavigatingToRetrievable);
                }
                break;
            case State.NavigatingToRetrievable:
                if (currentRetrievable == null)
                {
                    SwitchState(State.SearchingForRetrievable);
                } else if (!mover.IsNavigating())
                {
                    SwitchState(State.HaulingResources);
                }
                break;
            case State.HaulingResources:
                if (!mover.IsNavigating())
                {
                    resourceInventory.TryGiveAllResources(Stockpile.Instance.GetResourceInventory());
                    SwitchState(State.SearchingForRetrievable);
                }
                break;
        }
    }

    private void SwitchState(State newState)
    {
        // Exit
        switch (state)
        {
            case State.SearchingForRetrievable:
                break;
            case State.NavigatingToRetrievable:
                mover.Stop();
                break;
            case State.HaulingResources:
                OnHaulingCompleted?.Invoke();
                break;
        }

        state = newState;

        // Enter
        switch (state)
        {
            case State.SearchingForRetrievable:
                break;
            case State.NavigatingToRetrievable:
                mover.MoveTo(currentRetrievable.GetPosition());
                break;
            case State.HaulingResources:
                OnHaulingStarted?.Invoke();
                RetrievableManager.Instance.PickupRetrievable(currentRetrievable, resourceInventory);
                mover.MoveTo(Stockpile.Instance.GetPosition());
                break;
        }
    }

    public override void StartTask(TaskArgs e, Action onTaskComplete)
    {
        base.StartTask(e, onTaskComplete);
        currentRetrievable = (e as RetrieveTaskArgs).retrieveable;
        SwitchState(State.NavigatingToRetrievable);
    }

    public override void TaskComplete()
    {
        base.TaskComplete();
    }

    public override void CancelTask()
    {
        switch(state)
        {
            case State.HaulingResources:
                RetrievableManager.Instance.PlaceRetrievable(transform.position, resourceInventory);
                OnHaulingCompleted?.Invoke();
                break;
        }

        mover.Stop();
        base.CancelTask();
    }
}
