using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Damage Boost", order = 2)]
public class DamageBoost : Powerup
{
    // Start is called before the first frame update
    public int amount;

    public override void Activate(GameObject parent)
    {
        parent.GetComponent<PlayerStats>().attackDamage += amount;

    }

    public override void Deactivate(GameObject parent)
    {
        parent.GetComponent<PlayerStats>().attackDamage -= amount;
    }
}
