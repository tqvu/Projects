using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPool : MonoBehaviour
{
    public PlayerStats stats;

    private void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            stats.Healing(1000);
        }
    }
}
