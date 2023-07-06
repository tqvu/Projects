using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "Scriptable Objects/Health Boost", order = 2)]
public class ShopHealthBoost : ShopItems
{
    public int amount;

    public override void Activate(GameObject parent)
    {
        parent.GetComponent<PlayerStats>().smallPowerups[ID].enabled = true;
        parent.GetComponent<PlayerStats>().maxHealth += amount;
        parent.GetComponent<PlayerStats>().currentHealth += amount;
        LevelUp();
    }

    public void LevelUp()
    {
        if(level <= maxLevel)
        {
            level += 1;
            baseCost *= 10 / level;
        }
        else
        {
            purchasable = false;
        }
    }
}
