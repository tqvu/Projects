using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Stopwatch : MonoBehaviour
{
    float currentTime;
    public TextMeshProUGUI currentTimeText;
    public Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        currentTimeText = GameObject.FindGameObjectWithTag("Stopwatch").transform.Find("TimeText").GetComponent<TextMeshProUGUI>();
        toggle = GameObject.FindGameObjectWithTag("Stopwatch").transform.Find("StopwatchToggle").GetComponent<Toggle>();
        toggle.isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Contains("Zone") && !toggle.isOn)
        {
            Debug.Log("toggle is now on");
            toggle.isOn = true;
        }

        if((SceneManager.GetActiveScene().name.Contains("Hub") || 
            SceneManager.GetActiveScene().name.Contains("F2_Rest")) && 
            toggle.isOn)
        {
            toggle.isOn = false;
        }

        if (toggle.isOn)
        {
            currentTime = currentTime + Time.deltaTime;
        }
        else
        {
            currentTime = 0;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeText.text = time.ToString(@"mm\:ss");
    }
}
