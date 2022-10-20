using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    public event Action<ResourceType,int,int> OnResourceAmountChanged; // type, old, new

    [SerializeField] private int totalResourceLimit = int.MaxValue;

    private Dictionary<ResourceType, int> resourceDictionary;
    private int totalResourceAmount = 0;

    private void Awake()
    {
        resourceDictionary = new Dictionary<ResourceType, int>();
        ClearReasourceInventory();
    }

    public void ClearReasourceInventory()
    {
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            resourceDictionary[resourceType] = 0;
            OnResourceAmountChanged?.Invoke(resourceType, 0, 0);
        }
    }

    public bool TryAddResourceAmount(ResourceType resourceType, int amount)
    {
        if (IsFull()) return false;
        if (totalResourceAmount + amount > totalResourceLimit) return false;

        int oldValue = resourceDictionary[resourceType];

        resourceDictionary[resourceType] += amount;
        totalResourceAmount += amount;

        OnResourceAmountChanged?.Invoke(resourceType, oldValue, resourceDictionary[resourceType]);

        return true;
    }

    public bool TryRemoveResourceAmount(ResourceType resourceType, int amount)
    {
        if (GetResourceAmount(resourceType) < amount) return false;

        int oldValue = resourceDictionary[resourceType];

        resourceDictionary[resourceType] -= amount;
        totalResourceAmount -= amount;

        OnResourceAmountChanged?.Invoke(resourceType, oldValue, resourceDictionary[resourceType]);

        return true;
    }

    public int GetResourceAmount(ResourceType resourceType)
    {
        return resourceDictionary[resourceType];
    }

    public void TakeAllResources(ResourceInventory from)
    {
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            int amount = from.GetResourceAmount(resourceType);
            from.TryRemoveResourceAmount(resourceType, amount);
            TryAddResourceAmount(resourceType, amount);
        }
    }

    public bool TryGiveAllResources(ResourceInventory to)
    {
        bool gaveResources = false;

        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            int amount = Mathf.Min(GetResourceAmount(resourceType), to.GetTotalResourceLimit());
            if (amount == 0) continue;

            if (!to.TryAddResourceAmount(resourceType, amount))
                continue;

            TryRemoveResourceAmount(resourceType, amount);

            gaveResources |= true;
        }

        return gaveResources;
    }

    public bool TryGiveOneResource(ResourceInventory to)
    {
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            if (resourceDictionary[resourceType] == 0) continue;
            if (!TryRemoveResourceAmount(resourceType, 1)) continue; // Can't get one of this type skipping type

            if (!to.TryAddResourceAmount(resourceType, 1)) // Failed to add type to new inventory
            {
                TryAddResourceAmount(resourceType, 1); // Putting it back and skipping type
                continue;
            }

            return true;
        }

        return false;
    }

    public bool ContainsResource(ResourceType resourceType)
    {
        return resourceDictionary[resourceType] > 0;
    }

    public bool IsEmpty()
    {
        return totalResourceAmount == 0;
    }

    public bool IsFull()
    {
        return totalResourceAmount == totalResourceLimit;
    }

    public int GetTotalResourceAmount()
    {
        return totalResourceAmount;
    }

    public int GetTotalResourceLimit()
    {
        return totalResourceLimit;
    }
}