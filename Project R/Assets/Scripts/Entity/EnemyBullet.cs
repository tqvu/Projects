using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.tag.Equals("Enemy") && !collision.gameObject.tag.Equals("Player"))//goes through enemies and does trigger on player
        {
            Erase();
        }
        else if (collision.collider.CompareTag("Projectile"))
        {
            Erase();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            Erase();
        }
    }
    public void Erase()
    {
        Destroy(gameObject);
    }
}
