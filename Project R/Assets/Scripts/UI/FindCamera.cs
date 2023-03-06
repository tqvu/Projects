using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCamera : MonoBehaviour
{
    public void Awake()
    {
        GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>();
    }

    public void Update()
    {
        Awake();
    }
}
