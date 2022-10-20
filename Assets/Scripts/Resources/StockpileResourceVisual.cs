using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockpileResourceVisual : MonoBehaviour
{
    [SerializeField] private int maxGameObjectsPerResource = 50;
    [SerializeField] private Transform woodStockpile;
    [SerializeField] private Transform rockStockpile;


    private Dictionary<ResourceType, List<Transform>> resourceGameObjectDictionary;

    private void Awake()
    {
        resourceGameObjectDictionary = new Dictionary<ResourceType, List<Transform>>();

        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            resourceGameObjectDictionary[resourceType] = new List<Transform>();
        }
    }

    private void Start()
    {
        Stockpile.Instance.GetResourceInventory().OnResourceAmountChanged += Stockpile_OnResourceAmountChanged;
    }

    private void Stockpile_OnResourceAmountChanged(ResourceType resourceType, int oldAmount, int newAmount)
    {
        bool resourcesWereAdded = newAmount > oldAmount;
        int change = Mathf.Abs(newAmount - oldAmount);

        if (resourcesWereAdded && resourceGameObjectDictionary[resourceType].Count < maxGameObjectsPerResource)
        {
            for (int i = 0; i < change; i++)
            {
                AddResourceVisual(resourceType);
            }
        }
        else if (!resourcesWereAdded && newAmount < maxGameObjectsPerResource)
        {
            for (int i = 0; i < change; i++)
            {
                RemoveResourceVisual(resourceType);
            }
        }
    }

    private void AddResourceVisual(ResourceType resourceType)
    {
        List<Transform> transformList = resourceGameObjectDictionary[resourceType];
        RaycastHit hitInfo = Stockpile.Instance.GetRandomPoint();
        Transform visual = Instantiate(FindResourceTypePrefab(resourceType), hitInfo.point, Quaternion.identity, transform);

        visual.up = hitInfo.normal;

        transformList.Add(visual);
    }

    private void RemoveResourceVisual(ResourceType resourceType)
    {
        List<Transform> transformList = resourceGameObjectDictionary[resourceType];
        if (transformList.Count == 0) return;

        Destroy(transformList[0].gameObject);
        transformList.Remove(transformList[0]);
    }

    private Transform FindResourceTypePrefab(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Wood:
                return woodStockpile;
            case ResourceType.Rock:
                return rockStockpile;
        }

        return null;
    }
}
