using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Money", order = 5)]
public class IncreaseMoney : Powerup
{
    public int amount;

    public override void Activate(GameObject parent)
    {
        GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<CurrencyManager>(true).ChangeCurrency(amount);

    }
}
