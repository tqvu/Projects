using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currency;
    public int attackDamage;
    public int maxHealth;
    public int maxAmmo;
    public bool savedata;

    [Serializable]
    public class PowerupsData
    {
        public int ID;
        public int level;
        public bool enabled;
        public PowerupsData(int _ID, int _level, bool _enabled) 
        {
            ID = _ID;
            level = _level;
            enabled = _enabled;
        }
        
    }
    public List<PowerupsData> powerups = new List<PowerupsData>();


    //spapwn in hub with current data on load
    public PlayerData (PlayerStats player)
    { 
        currency = player.currency;
        attackDamage = player.attackDamage;
        maxHealth = player.maxHealth;
        maxAmmo = player.maxAmmo;

        foreach(ShopItems item in player.smallPowerups)
        {
            powerups.Add(new PowerupsData(item.ID, item.level, item.enabled));
        }

    }



}
