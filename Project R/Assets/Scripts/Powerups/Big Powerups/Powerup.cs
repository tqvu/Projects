using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu (menuName = "Powerups/Base Powerup", order = 0)]
public class Powerup : ScriptableObject
{
    public string title;
    public string description;
    public int ID;
    public bool enabled;

    public Sprite sprite;

    public virtual void Activate(GameObject parent) {}

    public virtual void Deactivate(GameObject parent) {}
    
}
