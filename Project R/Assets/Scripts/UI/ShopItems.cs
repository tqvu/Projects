using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "Scriptable Objects/Shop Item", order = 1)]
[System.Serializable]
public class ShopItems : ScriptableObject
{
    public string title;
    public string description;
    public int level = 1;
    public int maxLevel;
    public int ID;
    public bool enabled = false;
    public int baseCost;
    public bool purchasable = true;
    public Sprite sprite;

    public virtual void Activate(GameObject parent) { }
    
}
