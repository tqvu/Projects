using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Animator animator;
    public float kbPower = 1f;
    public int damage;
    public Rigidbody2D rb;


    public void Start()
    {
        if (FindObjectOfType<PlayerStats>().smallPowerups[4].enabled)
        {
            damage *= 2;
        }
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy Projectile"))
        {
            Destroy(collision.gameObject);
        }
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Erase();
        }
    }

    public IEnumerator kbCoroutine(Rigidbody2D tag, float kbTime)
    {
        animator.SetTrigger("Impact");
        FindObjectOfType<AudioManager>().Play("Fireball Impact");
        if (tag != null)
        {
            yield return new WaitForSeconds(kbTime);
            tag.velocity = Vector2.zero;
        }
        else { yield return null; }
        

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy Projectile"))
        {
            Erase();
        }
    }

    public void Erase()
    {
        Destroy(gameObject);
    }
}
