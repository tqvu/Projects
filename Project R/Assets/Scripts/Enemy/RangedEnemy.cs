using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Components")]
    public Transform firePoint;
    public RangedAttack rangedAttack;
    public Vector2 hingeBase;
    public Vector3 lightScale;
    public GameObject bulletPrefab;

    [Header("Stats")]
    public float aggroRange;
    public float fireForce;
    public float fireRate;

    [SerializeField] private bool canFire = true;
    [SerializeField] private bool canRun = false;

    // Start is called before the first frame update

    // Update is called once per frame
    public override void Start()
    {
        base.Start();
        hingeBase = rangedAttack.transform.localPosition;
        lightScale = transform.GetChild(0).transform.localScale;
    }

    protected override void FixedUpdate()
    {
        if (!enemyHurt && Health > 0)
        {
            if (spriteRend.flipX)//flip firing hinge
            {
                rangedAttack.transform.localPosition = new Vector2(-hingeBase.x, hingeBase.y);
                transform.GetChild(0).transform.localScale = new Vector3(-lightScale.x, lightScale.y, lightScale.z);
            }
            else
            {
                rangedAttack.transform.localPosition = hingeBase;
                transform.GetChild(0).transform.localScale = lightScale;
            }
            
            CheckDistance();
            
        }
    }

    public void PlaySkeletonSound1()
    {
        FindObjectOfType<AudioManager>().Play("Skeleton Walk1");
    }
    public void PlaySkeletonSound2()
    {
        FindObjectOfType<AudioManager>().Play("Skeleton Walk2");
    }
    public override void CheckDistance()
    {
        if (Vector2.Distance(PointerInput, transform.position) <= chaseRadius && canRun)//if player is in sweet zone
        {
            transform.position = Vector2.MoveTowards(transform.position, PointerInput, (-1) * moveSpeed * Time.fixedDeltaTime);
            if (transform.position.x > PointerInput.x) { print("True"); spriteRend.flipX = true; }//look away from player when running
            else { spriteRend.flipX = false; }

            Debug.Log("Too Close");
            
        }
        else if(Vector2.Distance(PointerInput, transform.position) > aggroRange && rb.velocity == Vector2.zero)//if skele isnt moving and is out of aggro range
        {
            if (canFire)
            {
                Invoke("MoveBack", 3f);
            }
        }
        else//if player is in attack range but out of aggro range
        {
            rb.velocity = MoveInput.normalized * moveSpeed;//chase player, if at certain range, then fire
        }
        
    }

    public void Shoot()
    {
        if (canFire && Health > 0)//shoots if it isnt firing and sets cd
        {
            animator.SetTrigger("attack");
        }
    }

    public IEnumerator FiringCooldown()
    {
        yield return new WaitForSeconds(1 / fireRate);//the cooldown
        canFire = true;
    }

    void Fire()
    {
        //Debug.Log(PointerInput);
        
        UnlockMovement();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponentInChildren<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        Destroy(bullet, 5);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    public void LockMovement()
    {
        Vector2 difference = PointerInput - (Vector2)rangedAttack.transform.position;
        float aimAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90f;//aiming code
        rangedAttack.body.rotation = aimAngle;

        if (PointerInput.x > transform.position.x) { spriteRend.flipX = true; }//point at player
        else { spriteRend.flipX = false; }
        canRun = false;
        canFire = false;
        rb.velocity = Vector2.zero;//stop moving
        StartCoroutine(FiringCooldown());
    }

    public void UnlockMovement()
    {
        canRun = true;
    }
}
