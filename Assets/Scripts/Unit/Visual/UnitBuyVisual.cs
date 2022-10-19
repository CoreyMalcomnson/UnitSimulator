using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitBuyVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text buyText;

    private void Start()
    {
        UnitBuyManager.Instance.OnUnitCreditCostChanged += UnitBuyManager_OnUnitCreditCostChanged;
        UpdateBuyText();
    }

    private void OnDestroy()
    {
        UnitBuyManager.Instance.OnUnitCreditCostChanged -= UnitBuyManager_OnUnitCreditCostChanged;
    }

    private void UnitBuyManager_OnUnitCreditCostChanged()
    {
        UpdateBuyText();
    }

    private void UpdateBuyText()
    {
        buyText.text = $"Buy c{UnitBuyManager.Instance.GetUnitCreditCost()}";
    }

}
