using System;
using System.Collections;
using UnityEngine;

public class TaskArgs
{
    public static readonly TaskArgs Empty;
}

public class HarvestTaskArgs : TaskArgs
{
    public Harvestable harvestable;
}

public class RetrieveTaskArgs : TaskArgs
{
    public Retrievable retrieveable;
}

public class MoveTaskArgs : TaskArgs
{
    public Vector3 position;
}