using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrievableManager : MonoBehaviour
{
    public static RetrievableManager Instance;
    
    [SerializeField] private Retrievable retrieveablePrefab;

    private List<Retrievable> retrievableList;

    private void Awake()
    {
        Instance = this;

        retrievableList = new List<Retrievable>();
    }

    public Retrievable PlaceRetrievable(Vector3 position, ResourceInventory sourceInventory)
    {
        if (sourceInventory.IsEmpty()) return null;

        Retrievable retrievable = Instantiate(Instance.retrieveablePrefab, transform);
        retrievable.transform.position = position;
        sourceInventory.GiveAllResources(retrievable.GetResourceInventory());
        retrievableList.Add(retrievable);

        return retrievable;
    }

    public void PickupRetrievable(Retrievable retrievable, ResourceInventory targetInventory)
    {
        retrievable.GetResourceInventory().GiveAllResources(targetInventory);
        retrievableList.Remove(retrievable);
        Destroy(retrievable.gameObject);
    }

    public Retrievable FindClosestRetrievable(Vector3 currentPosition)
    {
        Retrievable closestRetrievable = null;
        float smallestDistance = float.MaxValue;

        foreach (Retrievable retrievable in retrievableList)
        {
            float distance = (currentPosition - retrievable.GetPosition()).magnitude;
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closestRetrievable = retrievable;
            }
        }

        return closestRetrievable;
    }
}
