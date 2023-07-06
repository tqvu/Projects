using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    Vector2 leftAttackOffset;

    [Header("Attack Stats")]
    public int attackDamage;
    public float attackRange;

    [Header("Borrowed Components")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Transform attackPoint;
    public Enemy enemy;
    public LayerMask playerLayer;
    

    private IEnumerator coroutine;
    


    private void Start()
    {
        leftAttackOffset = attackPoint.transform.localPosition;
    }

    public void Attack()
    { 
        if(enemy.Health <= 0) { Destroy(gameObject); return; }
        //Debug.Log("attack");
        if (spriteRenderer.flipX == true)
        {
            attackPoint.transform.localPosition = new Vector2(-leftAttackOffset.x, leftAttackOffset.y);
        }
        else
        {
            attackPoint.transform.localPosition = leftAttackOffset;
        }

        animator.SetTrigger("Attacking");
        Collider2D[] player = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        
        
        foreach(Collider2D p in player)
        {
            CollisionHandler collision = p.gameObject.GetComponent<CollisionHandler>();
            PlayerControls controls = p.gameObject.GetComponent<PlayerControls>();
            PlayerStats stats = p.gameObject.GetComponent<PlayerStats>();
            Vector2 difference = p.transform.position - transform.position;
            difference = difference.normalized * collision.thrust;

            if(controls.canDash && !stats.hurt)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = collision.kbCoroutine(controls.body);

                stats.DamageTaken(attackDamage);//change to enemy attack damage

                //difference = (controls.body.mass * difference) / Time.fixedDeltaTime;
                Debug.Log(difference);
                controls.body.AddForce(difference, ForceMode2D.Impulse);
                StartCoroutine(coroutine);
            }
        }
            
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) { return; }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
