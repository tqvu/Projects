using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Health Boost", order = 1)]
public class HealthBoost : Powerup
{
    public int amount;

    public override void Activate(GameObject parent)
    {
        parent.GetComponent<PlayerStats>().maxHealth += amount;
        parent.GetComponent<PlayerStats>().currentHealth += amount;

    }

    public override void Deactivate(GameObject parent)
    {
        parent.GetComponent<PlayerStats>().maxHealth -= amount;
    }
}
