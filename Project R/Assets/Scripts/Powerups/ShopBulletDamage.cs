using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "Scriptable Objects/Bullet Damage Boost")]
public class ShopBulletDamage : ShopItems
{
    public int amount;

    public override void Activate(GameObject parent)
    {
        parent.GetComponent<PlayerStats>().smallPowerups[ID].enabled = true;
        LevelUp();
    }

    public void LevelUp()
    {
        if (level <= maxLevel)
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
