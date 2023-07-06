using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCounter : MonoBehaviour
{
    GameObject[] enemies;
    GameObject[] Exit;
    public TextMeshProUGUI text;

    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        InitializeExit();
    }
    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name == "Hub")
        {
            text.text = "";
        }
        else
        {
            EnemyCount();
        }
        
    }

    public void EnemyCount()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        text.text = "Enemies: " + enemies.Length;
        if (enemies.Length <= 0)
        {
            foreach(GameObject door in Exit)
            {
                if(door != null)
                {
                    door.SetActive(false);
                }
                
            }
        }
        else
        {
            InitializeExit();
        }
    }

    public void InitializeExit()
    {
        Exit = GameObject.FindGameObjectsWithTag("Exit");
        foreach (GameObject door in Exit)
        {
            door.SetActive(true);
        }
    }
}
