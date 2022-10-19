using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using CodeMonkey;

public class HarvestTask : BaseTask
{
    public event Action<Harvestable> OnHarvest;

    public event Action<Harvestable> OnHarvestingStarted;
    public event Action<Harvestable> OnHarvestingCompleted;

    public event Action<Harvestable> OnHaulingStarted;
    public event Action<Harvestable> OnHaulingCompleted;

    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float harvestDelay = 1f;
    [SerializeField] private float stoppingDistance = 3f;

    private enum State
    {
        SearchingForHarvestable,
        NavigatingToHarvestable,
        Harvesting,
        HaulingResources
    }

    private Unit unit;
    private Mover mover;
    private ResourceInventory resourceInventory;

    private State state;
    private Harvestable currentHarvestable;
    private HarvestableType harvestableType;
    private float timer;

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

        

        switch (state)
        {
            case State.SearchingForHarvestable:
                if (currentHarvestable == null)
                {
                    currentHarvestable = HarvestableManager.Instance.FindClosestHarvestable(transform.position, harvestableType);
                } else
                {
                    SwitchState(State.NavigatingToHarvestable);
                }
                break;
            case State.NavigatingToHarvestable:
                if (currentHarvestable.IsDead())
                {
                    SwitchState(State.SearchingForHarvestable);
                    break;
                }
                
                if (!mover.IsNavigating())
                {
                    SwitchState(State.Harvesting);
                }
                break;
            case State.Harvesting:
                // Hit timer
                if (timer <= 0)
                {
                    if (currentHarvestable.IsDead())
                    {
                        SwitchState(State.SearchingForHarvestable);
                        break;
                    }

                    timer = harvestDelay;
                    OnHarvest?.Invoke(currentHarvestable); //  This will trigger animation event which will call public TryHarvest function
                    print("Bam");
                }
                break;
            case State.HaulingResources:
                if (!mover.IsNavigating())
                {
                    SwitchState(State.SearchingForHarvestable);
                }
                break;
        }
    }

    // Called by various harvest functions
    public bool TryHarvest() // This could cause an issue if the harvestDelay is lower then the animation speed
    {
        if (currentHarvestable == null) return false;
        if (currentHarvestable.IsDead()) return false;

        currentHarvestable.Harvest();

        resourceInventory.TryAddResourceAmount(currentHarvestable.GetResourceType(), 1);

        if (resourceInventory.IsFull())
        {
            SwitchState(State.HaulingResources);
        }

        return true;
    }

    private void SwitchState(State newState)
    {
        // Exit
        switch (state)
        {
            case State.SearchingForHarvestable:
                break;
            case State.NavigatingToHarvestable:
                mover.Stop();
                break;
            case State.Harvesting:
                OnHarvestingCompleted?.Invoke(currentHarvestable);
                break;
            case State.HaulingResources:
                OnHaulingCompleted?.Invoke(currentHarvestable);
                resourceInventory.TryGiveAllResources(Stockpile.Instance.GetResourceInventory());
                break;
        }

        state = newState;

        // Enter
        switch (state)
        {
            case State.SearchingForHarvestable:
                currentHarvestable = null; // When u switch to this state
                break;
            case State.NavigatingToHarvestable:
                mover.MoveTo(currentHarvestable.GetPosition(), stoppingDistance);
                break;
            case State.Harvesting:
                timer = harvestDelay;
                OnHarvestingStarted?.Invoke(currentHarvestable);
                break;
            case State.HaulingResources:
                OnHaulingStarted?.Invoke(currentHarvestable);
                mover.MoveTo(Stockpile.Instance.GetPosition(), stoppingDistance);
                break;
        }
    }

    public override void StartTask(TaskArgs e, Action onTaskComplete)
    {
        base.StartTask(e, onTaskComplete);
        currentHarvestable = (e as HarvestTaskArgs).harvestable;// HarvestableManager.Instance.FindClosestHarvestable(transform.position, );
        harvestableType = currentHarvestable.GetHarvestableType();
        SwitchState(State.NavigatingToHarvestable);
    }



    public override void TaskComplete()
    {
        base.TaskComplete();
    }

    public override void CancelTask()
    {
        switch (state)
        {
            case State.Harvesting:
                break;
            case State.HaulingResources:
                OnHaulingCompleted?.Invoke(currentHarvestable);
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
