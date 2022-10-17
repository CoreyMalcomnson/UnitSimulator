using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retrievable : MonoBehaviour, ITaskable
{
    private ResourceInventory resourceInventory;

    private void Awake()
    {
        resourceInventory = GetComponent<ResourceInventory>();
    }

    public ResourceInventory GetResourceInventory()
    {
        return resourceInventory;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public TaskType GetTaskType()
    {
        return TaskType.Retrieve;
    }
}
