using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PowerupSelection : MonoBehaviour
{
    public List<Powerup> powerupPool = new List<Powerup>();//pool of all powerups to grab from and remove
                                                            //
    private List<Powerup> powerups = new List<Powerup>();//list of powerups to load
    public List<PowerupTemplate> powerupPanels = new List<PowerupTemplate>();//panels use this for enabling and disabling
    public List<Button> powerupButtons = new List<Button>();//list of buttons to select

    private bool shuffled = false;

    //select phase 
    //select
    //fade out and disable entire thing

    //grab objects from pool
    //cast the descriptions and other scriptable stuff into it
    //rng make sure there arent duplicates

    public void Start()
    {
        if (!shuffled)
        {
            AssignPowerups(); 
        }
        LoadPowerups();

    }


    public void AssignPowerups()
    {
        int powerup1 = SelectPowerup();
        int powerup2 = SelectPowerup();
        int powerup3 = SelectPowerup();

        while(powerup1 == powerup2 || powerup1 == powerup3 || powerup2 == powerup3)//picks from pool and makes sure that it doesnt grab the same one
        {
            while(powerup1 == powerup2)
            {
                powerup2 = SelectPowerup();
            }
            while(powerup1 == powerup3)
            {
                powerup3 = SelectPowerup();
            }
            while (powerup2 == powerup3)
            {
                powerup3 = SelectPowerup();
            }
        }

        //set each one active and handle assigning descriptors
        powerups.Add(powerupPool[powerup1]);
        powerups.Add(powerupPool[powerup2]);
        powerups.Add(powerupPool[powerup3]);
        shuffled = true;

    }

    public int SelectPowerup()
    {
        int powerupNumber = Random.Range(0, powerupPool.Count);
        while (powerupPool[powerupNumber] == null)
        {
            powerupNumber = Random.Range(0, powerupPool.Count);
        }
        return powerupNumber;
    }


    public void LoadPowerups()
    {
        for(int i = 0; i < powerupPanels.Count; i++)
        {
            powerupPanels[i].titleText.text = powerups[i].title;
            powerupPanels[i].descriptionText.text = powerups[i].description;
            powerupPanels[i].gameObject.GetComponent<Image>().sprite = powerups[i].sprite;
            powerupPanels[i].powerup = powerups[i];
        }
    }

    public void RemoveFromPool(int index)      
    {
        Powerup powerup = powerupPanels[index].gameObject.GetComponentInChildren<PowerupTemplate>(true).powerup;
        powerupPool.Remove(powerup);
    }

    public void ActivateUpgrade(int index)
    {
        Powerup powerup = powerupPanels[index].gameObject.GetComponentInChildren<PowerupTemplate>(true).powerup;
        powerup.Activate(GameObject.FindGameObjectWithTag("Player"));
        FindObjectOfType<PlayerStats>().AddPowerup(powerup);
    }

    public void CloseMenu()
    {
        shuffled = false;
        GameObject.FindGameObjectWithTag("NPC").GetComponent<UpgradeEnabler>().DisableUpgrades();
        Destroy(GameObject.FindGameObjectWithTag("NPC"));
    }
    //click -> animation plays -> icon appears in UI -> powerup activates -> spawn object on top of player -> ?? script activates


}
