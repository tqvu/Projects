using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "Scriptable Objects/Ammo Boost", order = 4)]
public class ShopAmmoBoost : ShopItems
{
    public int amount;

    public override void Activate(GameObject parent)
    {
        parent.GetComponent<PlayerStats>().smallPowerups[ID].enabled = true;
        parent.GetComponent<PlayerStats>().maxAmmo += amount;
        parent.GetComponent<PlayerStats>().currentAmmo += amount;
        LevelUp();
    }

    public void LevelUp()
    {
        if (level <= maxLevel)
        {
            level += 1;
            baseCost *= 5 / level;
        }
        else
        {
            purchasable = false;
        }
    }
}
