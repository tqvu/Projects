using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public bool isEnabled = false;

    public List<ShopItems> inventoryItems;
    public List<InventoryTemplate> inventoryPanels;
    public InventoryTemplate inventoryPrefab;

    void Awake()
    {
    }

    public void Update()
    {
        inventoryItems = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().smallPowerups;
        if (isEnabled && inventoryPanels.Count < inventoryItems.Count)
        { 
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                inventoryPanels.Add(Instantiate(inventoryPrefab, transform.GetChild(0).transform));
                //adds panels depending on how many items is in inventory
            }
            LoadPanels();
        }
        else if(!isEnabled && inventoryPanels.Count > 0)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                Destroy(inventoryPanels[i].gameObject);
            }
            
            inventoryPanels.Clear();
        }
    }


    public void LoadPanels()
    {
        for (int i = 0; i < inventoryPanels.Count; i++)
        {
            if (inventoryItems[i].enabled)
            {
                inventoryPanels[i].gameObject.GetComponent<Image>().sprite = inventoryItems[i].sprite;
                inventoryPanels[i].level.text = new string('I', inventoryItems[i].level - 1);
            }
            else
            {
                Color tmp = Color.white;
                tmp.a = .5f;
                inventoryPanels[i].gameObject.GetComponent<Image>().sprite = inventoryItems[i].sprite;
                inventoryPanels[i].gameObject.GetComponent<Image>().color = tmp;
            }
        }
    }
}
