using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Ammo Boost", order = 3)]
public class AmmoBoost : Powerup
{
    public int amount;

    public override void Activate(GameObject parent)
    {
        parent.GetComponent<PlayerStats>().maxAmmo += amount;
        parent.GetComponent<PlayerStats>().currentAmmo += amount;

    }

    public override void Deactivate(GameObject parent)
    {
        parent.GetComponent<PlayerStats>().maxAmmo -= amount;
    }
}
