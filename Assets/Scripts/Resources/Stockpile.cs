using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stockpile : MonoBehaviour, ITaskable
{
    public static Stockpile Instance;

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
        return transform.position;
    }

    public RaycastHit GetRandomPoint()
    {
        Vector3 randomPosition = transform.position + new Vector3(
            Random.Range(-randomPointExtent, randomPointExtent),
            2f,
            Random.Range(-randomPointExtent, randomPointExtent)
        );

        Physics.Raycast(randomPosition, Vector3.down, out RaycastHit hitInfo, 5f);

        return hitInfo;
    }

    public TaskType GetTaskType()
    {
        return TaskType.Sell;
    }
}
