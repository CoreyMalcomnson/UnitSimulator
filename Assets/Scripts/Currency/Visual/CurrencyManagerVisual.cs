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

        bool hasSymbol = GetSymbol(currencyType, out string symbol);
        string name = hasSymbol ? symbol : System.Enum.GetName(typeof(CurrencyType), currencyType) + "s: ";

        currencyTextDictionary[currencyType].text = $"{name}{amount}";
    }

    private bool GetSymbol(CurrencyType currencyType, out string symbol)
    {
        symbol = "";

        switch (currencyType)
        {
            case CurrencyType.Credit:
                symbol = "c";
                return true;
        }

        return false;
    }
}