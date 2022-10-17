using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManagerVisual : MonoBehaviour
{
    [SerializeField] Transform currencyContainer;
    [SerializeField] TMP_Text currencyTextPrefab;

    private Dictionary<CurrencyType, TMP_Text> currencyTextDictionary;

    private void Awake()
    {
        currencyTextDictionary = new Dictionary<CurrencyType, TMP_Text>();
    }

    private void Start()
    {
        foreach (CurrencyType currencyType in System.Enum.GetValues(typeof(CurrencyType)))
        {
            currencyTextDictionary[currencyType] = Instantiate(currencyTextPrefab, currencyContainer);
            UpdateResourceText(currencyType);
        }

        CurrencyManager.Instance.OnCurrencyAmountChanged += CurrencyManager_OnCurrencyAmountChanged;
    }
    private void OnDestroy()
    {
        CurrencyManager.Instance.OnCurrencyAmountChanged -= CurrencyManager_OnCurrencyAmountChanged;
    }

    private void CurrencyManager_OnCurrencyAmountChanged(CurrencyType currencyType)
    {
        UpdateResourceText(currencyType);
    }

    private void UpdateResourceText(CurrencyType currencyType)
    {
        int amount = CurrencyManager.Instance.GetCurrencyAmount(currencyType);
        currencyTextDictionary[currencyType].text = $"{System.Enum.GetName(typeof(CurrencyType), currencyType)}s: {amount}";
    }
}