using System;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public abstract class Harvestable : MonoBehaviour, ITaskable
{
    public event Action OnHarvestableEnabled;
    public event Action OnHarvestableDisabled;
    public event Action OnHarvest;

    [SerializeField] protected float respawnTime;
    [SerializeField] protected new Collider collider;

    protected HealthSystem healthSystem;

    protected virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void Start()
    {
        HarvestableManager.Instance.AddHarvestable(this);
    }

    protected virtual void OnDestroy()
    {
        healthSystem.OnDeath -= HealthSystem_OnDeath;
    }

    protected virtual  void HealthSystem_OnDeath(object sender, System.EventArgs e)
    {
        DisableHarvestable();
        Invoke(nameof(EnableHarvestable), respawnTime);
    }
    
    public virtual void Harvest()
    {
        healthSystem.Damage(1);
        OnHarvest?.Invoke();
    }

    protected virtual void DisableHarvestable()
    {
        collider.enabled = false;
        OnHarvestableDisabled?.Invoke();
    }

    protected virtual void EnableHarvestable()
    {
        collider.enabled = true;
        healthSystem.HealMax();
        OnHarvestableEnabled?.Invoke();
    }


    public bool IsDead()
    {
        return healthSystem.IsDead();
    }

    public TaskType GetTaskType()
    {
        return TaskType.Harvest;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public abstract string GetHarvestableName();
    public abstract HarvestableType GetHarvestableType();
    public abstract ResourceType GetResourceType();
}