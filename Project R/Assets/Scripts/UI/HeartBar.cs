using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerStats _playerStats;

    List<Heart> hearts = new List<Heart>();

    private void OnEnable()
    {
        PlayerStats.OnPlayerDamaged += DrawHearts;
        PlayerStats.OnPlayerHeal += DrawHearts;
    }
    private void OnDisable()
    {
        PlayerStats.OnPlayerDamaged -= DrawHearts;
        PlayerStats.OnPlayerHeal -= DrawHearts;
    }

    //private void OnLevelWasLoaded()
    //{
    //    DrawHearts();
    //}
    private void Start()
    {
        _playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        _playerStats.currentHealth = _playerStats.maxHealth;
        DrawHearts();
    }
    private void Update()
    {
        if(_playerStats == null)
        {
           // _playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }
        DrawHearts();

    }

    public void DrawHearts()
    {
        ClearHearts();

        float maxHealthRemainder = _playerStats.maxHealth % 2;//checks how many half hearts to add to the end
        int heartsToMake = (int)((_playerStats.maxHealth / 2) + maxHealthRemainder);
        for(int i = 0; i < heartsToMake; i++)//create empty heart shell depending on hp
        {
            CreateEmptyHeart();
        }

        for(int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = Mathf.Clamp(_playerStats.currentHealth - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }
    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);//instantiate prefab
        newHeart.transform.SetParent(transform);//set transform parent

        Heart heartComponent = newHeart.GetComponent<Heart>();//telling component to be empty and update sprite and list accordingly
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()//destroys everything under the parent object
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<Heart>();
    }






}

