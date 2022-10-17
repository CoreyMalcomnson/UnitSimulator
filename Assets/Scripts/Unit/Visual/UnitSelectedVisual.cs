using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private MeshRenderer meshRenderer;

    private void Start()
    {
        UnitSelectionManager.Instance.OnUnitSelected += UnitSelectionManager_OnUnitSelected;
        UnitSelectionManager.Instance.OnUnitDeselected += UnitSelectionManager_OnUnitDeselected;

        meshRenderer.enabled = UnitSelectionManager.Instance.IsUnitSelected(unit);
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.OnUnitSelected -= UnitSelectionManager_OnUnitSelected;
        UnitSelectionManager.Instance.OnUnitDeselected -= UnitSelectionManager_OnUnitDeselected;
    }

    private void UnitSelectionManager_OnUnitDeselected(Unit selectedUnit)
    {
        if (unit == selectedUnit) meshRenderer.enabled = false;
    }

    private void UnitSelectionManager_OnUnitSelected(Unit selectedUnit)
    {
        if (unit == selectedUnit) meshRenderer.enabled = true;
    }
}
