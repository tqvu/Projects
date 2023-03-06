using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Enemy slime;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.tag.Equals("Enemy") && !collision.gameObject.tag.Equals("Player"))//spawn cap
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < 11)
            {
                spawnSlime(gameObject.transform.position);
            }
            Erase();
        }
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().DamageTaken(1);
        }

    }

    public void Erase()
    {
        Destroy(gameObject);
    }

    public void spawnSlime(Vector3 position)
    {
        Instantiate(slime).transform.position = position;
    }
}
