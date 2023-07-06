using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionHandler : MonoBehaviour
{
    [Header("Stats")]
    [Range(1, 10)] public float thrust;
    public float kbTime = 0.2f;


    [Header("Borrowed Components")]
    public PlayerStats stats;
    public CurrencyManager currency;
    public MeleeController hitbox;
    public PlayerControls controls;

    IEnumerator coroutine;

    
    

    public void Update()
    {
        if(currency == null)
        {
            currency = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<CurrencyManager>(true);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            OnTriggerEnter2D(collision);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Enemy Projectile") || other.CompareTag("Trap"))
        {
            Vector2 difference = transform.position - other.transform.position;
            difference = difference.normalized * thrust; //(1, 0) - (3, 0) = (-2, 0) -> (-1, 0) * thrust (3) = (-3, 0) force
            if(other.CompareTag("Enemy Projectile"))
            {
                if (stats.hurt || controls.isDashing)//if during iframe or during roll frame
                {
                    Destroy(other.gameObject);
                }
            }
            if (!controls.isDashing && !stats.hurt)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = kbCoroutine(controls.body);

                if (other.CompareTag("Enemy"))
                {
                    Enemy enemy = other.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        if (enemy.attackDamage <= 0)
                        {
                            stats.DamageTaken(1);
                        }
                        else
                        {
                            stats.DamageTaken(enemy.attackDamage);
                        }
                    }
                    else
                    {
                        stats.DamageTaken(2);
                    }
                    
                }
                else if (other.CompareTag("Enemy Projectile"))
                {
                    if (controls.isDashing)
                    {
                        Destroy(other.gameObject);
                    }
                    else
                    {
                        EnemyBullet bullet = other.GetComponent<EnemyBullet>();
                        bullet.rb.velocity = Vector2.zero;
                        bullet.rb = null;
                        bullet.animator.SetTrigger("Impact");
                        stats.DamageTaken(1);
                    }
                    
                    
                    
                }
                else if (other.CompareTag("Trap"))
                {
                    Debug.Log("Trap");
                    difference = transform.position - other.transform.position;
                    difference = difference.normalized;

                    controls.body.AddForce(difference, ForceMode2D.Impulse);
                    StartCoroutine(kbCoroutine(controls.body));
                    stats.DamageTaken(1);
                    Destroy(other.gameObject);
                    //get other object
                    //damage character
                    //call coroutine
                    //destroy object
                }
                
                
               
                //difference = (controls.body.mass * difference) / Time.fixedDeltaTime;
                //Debug.Log(difference);
                controls.body.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(coroutine);
                
            }
            

        }
        if (other.gameObject.tag.Equals("Heal"))
        {
           stats.Healing(2);
            FindObjectOfType<AudioManager>().Play("Heart");
           Destroy(other.gameObject);
        }
        if (other.gameObject.tag.Equals("Coin"))
        {
            currency.ChangeCurrency(1);
            FindObjectOfType<AudioManager>().Play("Coin Collect");
            Destroy(other.gameObject);
        }

    }

    public IEnumerator kbCoroutine(Rigidbody2D tag)
    {
            controls.canMove = false;
            yield return new WaitForSeconds(kbTime);
            tag.velocity = Vector2.zero;
            controls.canMove = true;    
    }



}
