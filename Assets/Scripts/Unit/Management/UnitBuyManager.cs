using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBuyManager : MonoBehaviour
{
    public static UnitBuyManager Instance;

    public event Action OnUnitCreditCostChanged;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private int baseUnitCreditCost = 100;
    [SerializeField] private int unitCreditCostIncrease = 50;

    private int unitsBought = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(2)) return;
        if (!Physics.Raycast(MouseWorld.GetRay(), out RaycastHit hitInfo, float.MaxValue, groundLayerMask)) return;
        TryBuyUnit(hitInfo);
    }

    private bool TryBuyUnit(RaycastHit hitInfo)
    {
        if (!CurrencyManager.Instance.TryRemoveCurrencyAmount(CurrencyType.Credit, GetUnitCreditCost())) return false;

        UnitManager.Instance.SpawnUnit(hitInfo.point);
        unitsBought++;

        OnUnitCreditCostChanged?.Invoke();

        return true;
    }

    public int GetUnitCreditCost()
    {
        return baseUnitCreditCost + unitsBought * unitCreditCostIncrease;
    }
}
