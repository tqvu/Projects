using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Heal", order = 4)]
public class InstantHeal : Powerup
{
    public int amount;

    public override void Activate(GameObject parent)
    {
        parent.GetComponent<PlayerStats>().currentHealth += amount;

    }
}
