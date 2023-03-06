using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 shootDir;
    public float moveSpeed = 2f;

    public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir;
    }

    private void Update()
    {
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }
}
