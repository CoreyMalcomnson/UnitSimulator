using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Worker worker;
    private Mover mover;
    private ResourceInventory resourceInventory;

    private void Awake()
    {
        worker = GetComponent<Worker>();
        mover = GetComponent<Mover>();
        resourceInventory = GetComponent<ResourceInventory>();
    }

    private void Start()
    {
        UnitManager.Instance.AddUnit(this);        
    }

    private void OnDestroy()
    {
        UnitManager.Instance.RemoveUnit(this);
    }

    public Worker GetWorker()
    {
        return worker;
    }

    public Mover GetMover()
    {
        return mover;
    }

    public ResourceInventory GetResourceInventory()
    {
        return resourceInventory;
    }
}
