using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Harvestable
{
    public override string GetHarvestableName()
    {
        return "Tree";
    }

    public override HarvestableType GetHarvestableType()
    {
        return HarvestableType.Tree;
    }

    public override ResourceType GetResourceType()
    {
        return ResourceType.Wood;
    }
}
