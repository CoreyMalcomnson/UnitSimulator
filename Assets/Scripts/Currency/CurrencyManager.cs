using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public event Action<CurrencyType> OnCurrencyAmountChanged;

    private Dictionary<CurrencyType, int> currencyDictionary;

    private void Awake()
    {
        Instance = this;

        InitializeCurrencyDictionary();
    }

    private void Start()
    {
        AddCurrencyAmount(CurrencyType.Credit, 300);
    }

    private void InitializeCurrencyDictionary()
    {
        currencyDictionary = new Dictionary<CurrencyType, int>();

        foreach (CurrencyType currencyType in System.Enum.GetValues(typeof(CurrencyType)))
        {
            currencyDictionary[currencyType] = 0;
        }
    }

    public void AddCurrencyAmount(CurrencyType currencyType, int amount)
    {
        currencyDictionary[currencyType] += amount;
        OnCurrencyAmountChanged?.Invoke(currencyType);
    }

    public bool TryRemoveCurrencyAmount(CurrencyType currencyType, int amount)
    {
        if (GetCurrencyAmount(currencyType) < amount) return false;

        currencyDictionary[currencyType] -= amount;
        OnCurrencyAmountChanged?.Invoke(currencyType);

        return true;
    }

    public int GetCurrencyAmount(CurrencyType currencyType)
    {
        return currencyDictionary[currencyType];
    }
}
