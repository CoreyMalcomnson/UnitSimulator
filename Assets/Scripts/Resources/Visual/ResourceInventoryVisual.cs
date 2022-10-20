using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceInventoryVisual : MonoBehaviour
{
    [SerializeField] ResourceInventory resourceInventory;
    [SerializeField] Transform resourceContainer;
    [SerializeField] TMP_Text resourceTextPrefab;

    private Dictionary<ResourceType, TMP_Text> resourceTextDictionary;

    private void Awake()
    {
        resourceTextDictionary = new Dictionary<ResourceType, TMP_Text>();
    }

    private void Start()
    {
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            resourceTextDictionary[resourceType] = Instantiate(resourceTextPrefab, resourceContainer);
            UpdateResourceText(resourceType);
        }

        resourceInventory.OnResourceAmountChanged += ResourceInventory_OnResourceAmountChanged;
    }

    private void OnDestroy()
    {
        resourceInventory.OnResourceAmountChanged += ResourceInventory_OnResourceAmountChanged;
    }

    private void ResourceInventory_OnResourceAmountChanged(ResourceType resourceType, int oldValue, int newValue)
    {
        UpdateResourceText(resourceType);
    }

    private void UpdateResourceText(ResourceType resourceType)
    {
        int amount = resourceInventory.GetResourceAmount(resourceType);
        resourceTextDictionary[resourceType].text = $"{System.Enum.GetName(typeof(ResourceType), resourceType)}: {amount}";
    }
}
