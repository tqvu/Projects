using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyCaller : MonoBehaviour
{
    public CurrencyManager currency;

    public void Update()
    {
        currency = FindObjectOfType<ShopManager>(true).currencyManager;
    }

    public void ChangeCurrency()
    {
        currency.ChangeCurrency(1000);
    }
}
