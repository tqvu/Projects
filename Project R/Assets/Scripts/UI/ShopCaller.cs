using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCaller : MonoBehaviour
{

    public ShopManager shop;

    private void Update()
    {
        shop = GameObject.FindObjectOfType<ShopManager>(true);
    }

    public void CallShop()
    {        
        shop.EnableShop();
    }

    public void RemoveShop()
    {
        shop.DisableShop();
    }



}
