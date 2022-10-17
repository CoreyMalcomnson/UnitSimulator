using System;
using System.Collections.Generic;
using UnityEngine;

public class Stockpile : MonoBehaviour, ITaskable
{
    public static Stockpile Instance;

    [SerializeField] private Transform stockpileTransform;
    [SerializeField] private float randomPointExtent = 3.5f;

    private ResourceInventory resourceInventory;

    private void Awake()
    {
        Instance = this;

        resourceInventory = GetComponent<ResourceInventory>();
    }

    private void Start()
    {/*
        resourceInventory.TryAddResourceAmount(ResourceType.Wood, 500);
        resourceInventory.TryAddResourceAmount(ResourceType.Rock, 500);*/
    }

    public ResourceInventory GetResourceInventory()
    {
        return resourceInventory;
    }

    public Vector3 GetPosition()
    {
        return stockpileTransform.position;
    }

    public Vector3 GetRandomPoint()
    {
        return stockpileTransform.position + new Vector3(
            UnityEngine.Random.Range(-randomPointExtent, randomPointExtent),
            0,
            UnityEngine.Random.Range(-randomPointExtent, randomPointExtent));
    }

    public TaskType GetTaskType()
    {
        return TaskType.Sell;
    }
}
