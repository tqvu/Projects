using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindFollow : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;

    private void Awake()
    {
        VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        VirtualCamera.Follow = FindObjectOfType<PlayerControls>().transform;
    }
}
