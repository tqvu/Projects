using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "Scriptable Objects/Healing", order = 2)]
public class ShopHealing : ShopItems
{
    public int amount;

    public override void Activate(GameObject parent)
    {
        parent.GetComponent<PlayerStats>().smallPowerups[ID].enabled = true;
        //check for this ID powerup and see if its enabled
        LevelUp();
    }

    public void LevelUp()
    {
        if (level <= maxLevel)
        {
            level += 1;
            baseCost *= 2;
            amount *= level;
        }
        else
        {
            purchasable = false;
        }
    }
}
