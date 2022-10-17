using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private Animator animator;
    private Unit unit;
    private HarvestTask harvestTask;
    private SellTask sellTask;
    private Mover mover;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        unit = GetComponent<Unit>();
    }

    private void Start()
    {
        harvestTask = unit.GetWorker().GetTask<HarvestTask>();
        sellTask = unit.GetWorker().GetTask<SellTask>();
        mover = unit.GetMover();

        mover.OnNavigatingCompleted += Mover_OnNavigatingCompleted;
        mover.OnNavigatingStarted += Mover_OnNavigatingStarted;

        harvestTask.OnHarvest += HarvestTask_OnHarvest;

        sellTask.OnDelivering += SellTask_OnDelivering;
    }

    

    private void OnDestroy()
    {
        mover.OnNavigatingCompleted -= Mover_OnNavigatingCompleted;
        mover.OnNavigatingStarted -= Mover_OnNavigatingStarted;

        harvestTask.OnHarvest -= HarvestTask_OnHarvest;

        sellTask.OnDelivering -= SellTask_OnDelivering;
    }

    private void SellTask_OnDelivering()
    {
        animator.SetTrigger("Throw");
    }

    private void Mover_OnNavigatingCompleted()
    {
        animator.SetBool("IsNavigating", false);
    }

    private void Mover_OnNavigatingStarted()
    {
        animator.SetBool("IsNavigating", true);
    }

    private void HarvestTask_OnHarvest(Harvestable harvestable)
    {
        animator.SetTrigger("Harvest");

        HarvestableType harvestableType = harvestable.GetHarvestableType();
        animator.SetInteger("HarvestableEnum", (int)harvestableType);
    }
}
