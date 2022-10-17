using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    public event Action<Unit> OnUnitAdded;
    public event Action<Unit> OnUnitRemoved;

    private List<Unit> unitsList;

    private void Awake()
    {
        Instance = this;
        unitsList = new List<Unit>();
    }

    public void AddUnit(Unit unit)
    {
        unitsList.Add(unit);
        OnUnitAdded?.Invoke(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitsList.Remove(unit);
        OnUnitRemoved?.Invoke(unit);
    }

    public List<Unit> GetUnitsList()
    {
        return unitsList;
    }
}
