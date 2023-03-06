using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    public TextMeshProUGUI text;
    public TextMeshProUGUI shopText;
    public int currency;
    public ShopManager shop;

    public void Update()
    {
        shop = FindObjectOfType<ShopManager>(true);
        shopText = shop.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>(true);
        if (shopText.enabled)
        {
            shopText.text = "X" + currency.ToString();
        }
        text.text = "X" + currency.ToString();
    }
    public void ChangeCurrency(int currencyValue)
    {
        currency += currencyValue;
        text.text = "X" + currency.ToString();
        
        if (shop != null && shopText != null)
        {
            
            if (shop.isEnabled)
            {
                //shopText.text = "X" + currency.ToString();
                shop.CheckPurchasable();
            }
        }
        
        
    }
}
