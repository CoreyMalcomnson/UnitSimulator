using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHeldItems : MonoBehaviour
{
    [SerializeField] private GameObject axeHeld;
    [SerializeField] private GameObject treelogHeld;
    [SerializeField] private GameObject retrievableHeld;
    [SerializeField] private GameObject pickaxeHeld;
    [SerializeField] private GameObject rockHeld;

    private Unit unit;
    private HarvestTask harvestTask;
    private RetrieveTask retrieveTask;
    private SellTask sellTask;

    private GameObject currentHeld;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void Start()
    {
        harvestTask = unit.GetWorker().GetTask<HarvestTask>();
        retrieveTask = unit.GetWorker().GetTask<RetrieveTask>();
        sellTask = unit.GetWorker().GetTask<SellTask>();

        harvestTask.OnHarvestingStarted += HarvestTask_OnHarvestingStarted;
        harvestTask.OnHarvestingCompleted += HarvestTask_OnHarvestingCompleted;

        harvestTask.OnHaulingStarted += HarvestTask_OnHaulingStarted;
        harvestTask.OnHaulingCompleted += HarvestTask_OnHaulingCompleted;

        retrieveTask.OnHaulingStarted += RetrieveTask_OnHaulingStarted;
        retrieveTask.OnHaulingCompleted += RetrieveTask_OnHaulingCompleted;
    }

    

    private void OnDestroy()
    {
        harvestTask.OnHarvestingStarted -= HarvestTask_OnHarvestingStarted;
        harvestTask.OnHarvestingCompleted -= HarvestTask_OnHarvestingCompleted;

        harvestTask.OnHaulingStarted -= HarvestTask_OnHaulingStarted;
        harvestTask.OnHaulingCompleted -= HarvestTask_OnHaulingCompleted;

        retrieveTask.OnHaulingStarted -= RetrieveTask_OnHaulingStarted;
        retrieveTask.OnHaulingCompleted -= RetrieveTask_OnHaulingCompleted;
    }

    private void HarvestTask_OnHarvestingStarted(Harvestable harvestable)
    {
        switch (harvestable.GetHarvestableType())
        {
            case HarvestableType.Tree:
                Show(axeHeld);
                break;
            case HarvestableType.Rock:
                Show(pickaxeHeld);
                break;
        }
    }

    private void HarvestTask_OnHarvestingCompleted(Harvestable harvestable)
    {
        Hide();
    }

    private void HarvestTask_OnHaulingStarted(Harvestable harvestable)
    {
        switch (harvestable.GetHarvestableType())
        {
            case HarvestableType.Tree:
                Show(treelogHeld);
                break;
            case HarvestableType.Rock:
                Show(rockHeld);
                break;
        }
    }

    private void HarvestTask_OnHaulingCompleted(Harvestable harvestable)
    {
        Hide();
    }

    private void RetrieveTask_OnHaulingStarted()
    {
        Show(retrievableHeld);
    }

    private void RetrieveTask_OnHaulingCompleted()
    {
        Hide();
    }

    private void Show(GameObject newHeld)
    {
        Hide();

        currentHeld = newHeld;
        currentHeld.SetActive(true);
    }

    private void Hide()
    {
        if (currentHeld == null) return;

        currentHeld.SetActive(false);
        currentHeld = null;
    }

    public void ShowRetrievable()
    {
        Show(retrievableHeld);
    }

    public void HideRetrievable()
    {
        Hide();
    }
}
