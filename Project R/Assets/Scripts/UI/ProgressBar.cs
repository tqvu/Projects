using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode()]

public class ProgressBar : MonoBehaviour
{
    public float max;
    public float current;
    public Image mask;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        current = Time.time;
        float fillAmount = 1 - (current / max);
        mask.fillAmount = fillAmount;
    }
}
