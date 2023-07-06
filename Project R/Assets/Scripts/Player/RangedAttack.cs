using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Unity.VisualScripting;
using System;

public class RangedAttack : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float fireRate;
    [SerializeField] private float reloadTime;
    public float fireForce = 3f;
    [SerializeField] private float currentDelay;

    [Header("Bools")]
    public bool reloadTrigger = false;
    public bool facingLeft;
    [SerializeField] private bool reloading = false;
    [SerializeField] private bool firing = false;
    

    [Header("Borrowed Components")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Rigidbody2D body;
    public PlayerStats stats;
    public PlayerControls controls;
    public Animator animator;

    public UnityEvent<float> OnReloading;
    public static event Action OnPlayerFire;//action for ammo bar


    IEnumerator coroutine;

    Vector2 mousePosition;

    private void Start()
    {
        OnReloading?.Invoke(currentDelay);

    }
    void Update()
    {
        //add crosshair
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(stats != null && controls != null && (stats.currentAmmo <= 0 || reloadTrigger) && stats.currentAmmo != stats.maxAmmo)
        {
            if (!reloading) 
            {
                currentDelay = reloadTime;
                coroutine = Refill();
                StartCoroutine(coroutine);
            }
            if (reloading)
            {
                currentDelay -= Time.deltaTime;
                OnReloading?.Invoke(currentDelay / reloadTime);
            }

        }
    }

    private void FixedUpdate()
    {
        if (gameObject.tag.Equals("Player Ranged"))
        {
            Vector2 aimDirection = mousePosition - body.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
            body.rotation = aimAngle;
            if (controls.isDashing && coroutine != null)//reload cancelling
            {
                StopCoroutine(coroutine);
                OnReloading?.Invoke(0);
                reloading = false;
                reloadTrigger = false;
            }
        }
        
    }
    public void Fire()
    {
        facingLeft = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().flipX;
        if (!firing && !reloading && controls.canDash)
        {
            animator.SetTrigger("Fireball");//locks movement, then unlocks movement

            int num = UnityEngine.Random.Range(1, 3);
            switch (num)
            {
                case 1:
                    FindObjectOfType<AudioManager>().Play("Fireball1");
                    break;
                case 2:
                    FindObjectOfType<AudioManager>().Play("Fireball2");
                    break;

            }

            OnPlayerFire?.Invoke();
            if (FindObjectOfType<Crosshair>().transform.position.x > transform.position.x)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().flipX = false;
                GameObject.Find("MeleeWeapon").GetComponent<Animator>().SetTrigger("rangedAttack");
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().flipX = true;
                GameObject.Find("MeleeWeapon").GetComponent<Animator>().SetTrigger("rangedAttackFlip");
            }
            stats.currentAmmo--;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            Destroy(bullet, 1);
            StartCoroutine(FireRate());
        }
        
    }

    public IEnumerator Refill()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        stats.currentAmmo = stats.maxAmmo;
        reloading = false;
        reloadTrigger = false;
    }
    
    public IEnumerator FireRate()
    {
        firing = true;
        yield return new WaitForSeconds(1f / fireRate);
        firing = false;
    }
    


}
