using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/Speed Boost", order = 6)]
public class SpeedBoost : Powerup
{
    public int amount;

    public override void Activate(GameObject parent)
    {
        parent.GetComponent<PlayerControls>().baseMoveSpeed += amount;

    }

    public override void Deactivate(GameObject parent)
    {
        parent.GetComponent<PlayerControls>().baseMoveSpeed -= amount;
    }
}
