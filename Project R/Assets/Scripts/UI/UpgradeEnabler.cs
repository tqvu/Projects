using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEnabler : MonoBehaviour
{
    public PowerupSelection upgradeMenu;

    private void Update()
    {
        upgradeMenu = GameObject.FindGameObjectWithTag("Overlay UI").GetComponentInChildren <PowerupSelection>(true);
    }

    public void EnableUpgrades()
    {
        upgradeMenu.gameObject.SetActive(true);
    }

    public void DisableUpgrades()
    {
        upgradeMenu.gameObject.SetActive(false);
    }
}
