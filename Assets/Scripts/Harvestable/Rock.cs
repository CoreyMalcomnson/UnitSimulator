using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Harvestable
{
    public override string GetHarvestableName()
    {
        return "Rock";
    }

    public override HarvestableType GetHarvestableType()
    {
        return HarvestableType.Rock;
    }

    public override ResourceType GetResourceType()
    {
        return ResourceType.Rock;
    }
}
