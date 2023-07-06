using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool invulnerable = false;
    public Material flashMaterial;
    public Material originalMaterial;

    [Header("Stats")]
    public int health;
    public int maxHealth = 3;
    public float moveSpeed;
    public float chaseRadius;
    public int attackDamage;
    public GameObject[] drops;
    public enum EnemyType
    {
        Slime,
        Skeleton,
        Eye,
        Knight,
        Other
    }

    public EnemyType type;

    [Header("Movement")]
    [SerializeField]
    protected Vector2 moveInput;//move input
    [SerializeField]
    protected Vector2 pointerInput;//target ransform position
    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MoveInput { get => moveInput; set => moveInput = value; }
    public Vector3 homePosition;
    
    public Animator animator;

    [Header("Damage Indicator")]
    public float iFrameDuration;
    [SerializeField] int numberOfFlashes;
    public bool enemyHurt = false;
    public SpriteRenderer spriteRend;
    public bool isFlipped = false;

    IEnumerator coroutine;
    IEnumerator kbCoroutine;


    public int Health
    {
        set
        {
            health = value;
            if(health <= 0)
            {
                health = 0;
                animator.SetBool("Dead", true);
                animator.SetTrigger("enemyDeath");
                foreach (CircleCollider2D circle in gameObject.GetComponents<CircleCollider2D>())
                {
                    circle.enabled = false;
                }
                foreach (BoxCollider2D box in gameObject.GetComponents<BoxCollider2D>())
                {
                    box.enabled = false;
                }
                rb = null;
            }
        }
        get
        {
            return health;
        }
    }
    

     public virtual void Start()
     {
        Health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        homePosition = transform.position;
        originalMaterial = spriteRend.material;
     }

    protected virtual void FixedUpdate()
    {
        if (!enemyHurt && (type == EnemyType.Slime || type == EnemyType.Knight) && Health > 0)
        {
            rb.velocity = MoveInput.normalized * moveSpeed;
            if(rb.velocity != Vector2.zero) 
            {
                animator.SetBool("Moving", true);
            }
            else 
            { 
                animator.SetBool("Moving", false); 
            }
            if (PointerInput.x > transform.position.x) 
            { 
                spriteRend.flipX = true; 
            }
            else 
            { 
                spriteRend.flipX = false; 
            }
            CheckDistance();
        }
    }
    
    public void PlaySlimeSound()
    {
        if(Vector2.Distance((Vector2)transform.position, pointerInput) <= 1.5f)
        {
            int num = Random.Range(0, 2);
            if (num == 0)
            {
                FindObjectOfType<AudioManager>().Play("Slime Jump1");
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("Slime Jump2");
            }
        }
        
    }

    public void PlayKnightWalk1()
    {
        if (Vector2.Distance((Vector2)transform.position, pointerInput) <= 1.5f)
            FindObjectOfType<AudioManager>().Play("Knight Walk1");
    }
    public void PlayKnightWalk2()
    {
        if (Vector2.Distance((Vector2)transform.position, pointerInput) <= 1.5f)
            FindObjectOfType<AudioManager>().Play("Knight Walk2");
    }

    public void PlaySlimeDeath()
    {
         FindObjectOfType<AudioManager>().Play("Slime Death");
    }
    public void PlayKnightDeath()
    {
         FindObjectOfType<AudioManager>().Play("Knight Death");
    }
    public void PlaySkeletonDeath()
    {
         FindObjectOfType<AudioManager>().Play("Skeleton Death");
    }

    public IEnumerator Damaged()
    {
        enemyHurt = true;
        switch (type)
        {
            case EnemyType.Slime:
                FindObjectOfType<AudioManager>().Play("Slime Jump1");
                break;
            case EnemyType.Skeleton:
                int num = Random.Range(0, 2);
                if (num == 0)
                {
                    FindObjectOfType<AudioManager>().Play("Skeleton Hurt1");
                }
                else
                {
                    FindObjectOfType<AudioManager>().Play("Skeleton Hurt2");
                }
                break;
            case EnemyType.Eye:
                FindObjectOfType<AudioManager>().Play("Slime Death");
                break;
            case EnemyType.Knight:
                num = Random.Range(0, 2);
                if (num == 0)
                {
                    FindObjectOfType<AudioManager>().Play("Knight Hurt1");
                }
                else
                {
                    FindObjectOfType<AudioManager>().Play("Knight Hurt2");
                }
                break;
        }
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.material = flashMaterial;
            yield return new WaitForSeconds(iFrameDuration / numberOfFlashes);
            spriteRend.material = originalMaterial;
        }
        enemyHurt = false;

    }
    
    public virtual void CheckDistance()
    {
        if(Vector2.Distance(PointerInput, transform.position) > chaseRadius && rb.velocity == Vector2.zero)
        {
            Invoke("MoveBack", 1f);//1s delay before moving back
        }
        
    }

    public void MoveBack()
    {
        //Debug.Log("Moving Back");
        if (Vector2.Distance(PointerInput, transform.position) > chaseRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, homePosition, moveSpeed * Time.fixedDeltaTime);
            if(homePosition.x < transform.position.x)
            {
                spriteRend.flipX = false;
                isFlipped = false;
            }
            else if (homePosition == transform.position) 
            {
                spriteRend.flipX = false;
                isFlipped = false;
            }
            else
            {
                spriteRend.flipX = true;
                isFlipped = true;
            }
        }
        
    }

    void Death()
    {
        Vector3 targetLocation = new Vector3(transform.position.x + Random.Range(0.05f, 0.1f), transform.position.y + Random.Range(0.05f, 0.1f), 0);
        //Debug.Log(targetLocation);
        if (drops.Length != 0)
        {
            int index = Random.Range(0, drops.Length);
            GameObject drop = Instantiate(drops[index], targetLocation, Quaternion.identity);
            drop.SetActive(true);
        }
        
        
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            if(rb != null && !enemyHurt && !invulnerable)
            {
                Bullet bullet = collision.GetComponent<Bullet>();
                coroutine = Damaged();

                rb.velocity = Vector2.zero;
                Health -= bullet.damage;
                bullet.rb.velocity = Vector2.zero;
                bullet.GetComponent<CircleCollider2D>().enabled = false;

                kbCoroutine = bullet.kbCoroutine(rb, 0.2f);
                if (rb != null && bullet.rb != null)
                {
                    Vector2 difference = rb.transform.position - bullet.rb.transform.position;
                    difference = difference.normalized * bullet.kbPower;
                    rb.AddForce(difference, ForceMode2D.Impulse);
                }
                bullet.rb = null;
                
                
                StartCoroutine(coroutine);
                StartCoroutine(kbCoroutine);
                
                
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
        if (collision.CompareTag("Trap"))
        {
            if (!enemyHurt && !invulnerable && rb != null)
            {
                coroutine = Damaged();
                rb.velocity = Vector2.zero;
                Health -= 1;
                Destroy(collision.gameObject);
            }
        }
        if (collision.CompareTag("Enemy"))
        {
            if (enemyHurt)//checks if enemy is hurt and hurts both objects
            {
                Enemy other = collision.gameObject.GetComponent<Enemy>();

                if (other != null && rb != null)
                {
                    other.rb.velocity = Vector2.zero;
                    rb.velocity = Vector2.zero;
                }

                if (!other.enemyHurt && Health > 0)
                {
                    coroutine = other.Damaged();
                    other.Health -= 1;
                    StartCoroutine(coroutine);
                }

            }

        }
    }


}
