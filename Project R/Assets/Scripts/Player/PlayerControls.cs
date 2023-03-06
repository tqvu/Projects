using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerControls : MonoBehaviour
{
    [Header("Collider")]
    public float collisionOffset = 0.05f;
    [SerializeField] private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public ContactFilter2D movementFilter;
    [SerializeField] private RaycastHit2D dashCast;

    [Header("Movement")]
    public Vector2 moveInput;
    public float baseMoveSpeed = 50f;
    public float activeMoveSpeed;
    public float idleFriction = 0.9f;

    IEnumerator dashCoroutine;
    IEnumerator attackCoroutine;
    public float dashingPower = 24f;
    private Vector2 dashDirection;
    

    [Header("Bools")]
    public bool isDashing;
    public bool canDash = true;
    [SerializeField] private bool canAttack = true;
    public bool canMove = true;
    public bool isMoving = false;
    public bool isDeath = false;

    [Space(20)]
    public int floor_number = 1;
    public List<string> usedScenes = new List<string>();

    [Header("Borrowed Components")]
    public Rigidbody2D body;
    public MeleeController melee;
    public PauseMenu pause;
    public ShopManager shopUI;
    public GameObject prompt;
    public RangedAttack rangedAttack;
    public PlayerStats playerStats;
    public PowerupSelection upgradeMenu;

    public SpriteRenderer weaponRenderer, spriteRenderer;

    Animator animator;
    BoxCollider2D box;
    Vector2 boxOffset;

    //Vector 2 -> 2d Vector with X and Y speed

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        boxOffset = new Vector2(box.offset.x, box.offset.y / 1.33f);
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();//added some animation precode
        spriteRenderer = GetComponent<SpriteRenderer>();//sprite render for flipping sprite (no need for both left and right sprite now)
        activeMoveSpeed = baseMoveSpeed;
        upgradeMenu = FindObjectOfType<PowerupSelection>(true);



    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay((Vector2)transform.position + boxOffset, dashDirection / 1.33f);
        Gizmos.DrawWireSphere((Vector2)transform.position + boxOffset + dashDirection/1.33f, .14f);

    }

    public void PlayDashSound()
    {
        FindObjectOfType<AudioManager>().Play("Footstep Grass");
    }


    //movement
    void FixedUpdate()
    {
        if (FindObjectOfType<CinemachineVirtualCamera>().m_Lens.OrthographicSize >= 1.3f && FindObjectOfType<CinemachineVirtualCamera>().m_Lens.OrthographicSize >= 1.03f && !SceneManager.GetActiveScene().name.Contains("Boss"))
        {
            FindObjectOfType<CinemachineVirtualCamera>().m_Lens.OrthographicSize -= Time.deltaTime;
        }
        if (SceneManager.GetActiveScene().name.Contains("F"))
        {
            gameObject.GetComponent<Light2D>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Light2D>().enabled = false;
        }
        shopUI = FindObjectOfType<ShopManager>(true);
        pause = FindObjectOfType<PauseMenu>(true);
        upgradeMenu = FindObjectOfType<PowerupSelection>(true);
        if (canMove && !pause.isPaused && !isDeath)
        { 
            //movement speed modifier
            if (isDashing)
            {
                //Debug.Log(dashDirection);
                body.velocity = dashDirection.normalized * dashingPower;
                

                //dashCast = Physics2D.Raycast(body.transform.position, dashDirection.normalized, dashingPower/2, LayerMask.GetMask("Interactable"));

                if (Physics2D.OverlapPoint((Vector2)transform.position + boxOffset + dashDirection/1.33f, LayerMask.GetMask("Interactable")) !=  null)
                {
                    Physics2D.IgnoreLayerCollision(6, 8, false);
                    Physics2D.IgnoreLayerCollision(6, 7, false);
                }
                return;
            }

            // Try to move player in input direction, followed by left right and up down input if failed
            bool success = PlayerMovement(moveInput);

            //if player hits a wall check if player can move left/right or up/down the wall
            if (moveInput != Vector2.zero)
            {
                isMoving = true;
                if (!success)
                {
                    success = PlayerMovement(new Vector2(moveInput.x, 0));
                    print("Moving L/R");
                    if (!success)
                    {
                        print("Moving Up");
                        success = PlayerMovement(new Vector2(0, moveInput.y));
                    }
                }
                


                //flips sprite if you are moving left/right
                if (moveInput.x < 0)
                {
                    spriteRenderer.flipX = true;
                    GameObject.Find("MeleeWeapon").GetComponent<Animator>().SetBool("weaponFlip", true);
                }
                else if (moveInput.x > 0)
                {
                    GameObject.Find("MeleeWeapon").GetComponent<Animator>().SetBool("weaponFlip", false);
                    spriteRenderer.flipX = false;
                }

                animator.SetBool("isMoving", success);
            }
            else
            {
                isMoving = false;
                animator.SetBool("isMoving", false);
                body.velocity = Vector2.Lerp(body.velocity, Vector2.zero, 0.95f);
            }
        }
        
    }
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();    
        //gathers user movement inputs    
    }

    private bool PlayerMovement(Vector2 direction)
    {
        if(moveInput != Vector2.zero)
        {
            int count = body.Cast(
            direction,
            movementFilter,
            castCollisions,
            collisionOffset);

            if (count == 0)
            {
                Vector2 moveVector = direction.normalized * activeMoveSpeed;

                //No Collisions
                //body.MovePosition(body.position + moveVector);
                body.AddForce(moveVector, ForceMode2D.Force);

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
            
            
    }

    private IEnumerator Dash(float dashingTime, float dashingCooldown)
    {
        canDash = false;
        canAttack = false;
        isDashing = true;
        Physics2D.IgnoreLayerCollision(6, 8, true);
        Physics2D.IgnoreLayerCollision(6, 7, true);
        yield return new WaitForSeconds(dashingTime);//during dash
        Physics2D.IgnoreLayerCollision(6, 8, false);
        Physics2D.IgnoreLayerCollision(6, 7, false);
        isDashing = false;
        canAttack = true;
        activeMoveSpeed = baseMoveSpeed;
        yield return new WaitForSeconds(dashingCooldown);//wait dash cd
        canDash = true;
        
    }

    void OnDash()
    {
        
        if (canDash && !pause.isPaused && !isDeath && canMove)
        {
            animator.SetTrigger("isDashing");
            if (dashCoroutine != null)
            {
                //stop condition for coroutine if you are already dashing
                StopCoroutine(dashCoroutine);
            }

            if (moveInput == Vector2.zero)
            {
                if (spriteRenderer.flipX && moveInput.y == 0)//if sprite is facing left
                {
                    dashDirection = new Vector2(-1, 0);//move in set y direction
                }
                else if (!spriteRenderer.flipX && moveInput.y == 0)//if sprite is facing right
                {
                    dashDirection = new Vector2(1f, 0);//move in set x direction
                }
            }
            else
            {
                dashDirection = new Vector2(moveInput.x, moveInput.y).normalized;//will dash in diagonal movement
            }
            dashCoroutine = Dash(0.2f, 0.5f);//calls coroutine for dash manuever
            StartCoroutine(dashCoroutine);
        }
        else
        {
            return;
        }
    }

    void OnMelee()
    {
        if (canAttack && !pause.isPaused && canMove && !isDeath)
        {
            int num = Random.Range(1, 3);
            switch (num)
            {
                case 1:
                    FindObjectOfType<AudioManager>().Play("Sword Slash1");
                    break;
                case 2:
                    FindObjectOfType<AudioManager>().Play("Sword Slash2");
                    break;
            }
            
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            attackCoroutine = AttackCoroutine(melee.attackRate);
            canAttack = false;
            StartCoroutine(attackCoroutine);
        }
    }


    public void LockMovement()
    {
        canMove = false;
        isMoving = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    public void OnPause()
    {
        if (!shopUI.isEnabled && !upgradeMenu.isActiveAndEnabled && !isDeath)
        {
            if (!pause.isPaused)
            {
                pause.isPaused = !pause.isPaused;
                pause.Pause();
                body.velocity = Vector2.zero;
            }
            else
            {
                pause.isPaused = !pause.isPaused;
                pause.Resume();
                body.velocity = Vector2.Lerp(body.velocity, Vector2.zero, 0.9f);
            }
        }
        
    }

    public void PromptEnable()
    {
        prompt.SetActive(true);
    }
    public void PromptDisable()
    {
        prompt.SetActive(false);
    }

    private IEnumerator AttackCoroutine(float attackRate)
    {
        //GameObject.Find("MeleeWeapon").GetComponent<Animator>().SetTrigger("weaponAttack");
        if (spriteRenderer.flipX == true)
        {
            melee.Attack();
        }
        else
        {
            melee.Attack();
        }
        yield return new WaitForSeconds(1f / attackRate);
        canAttack = true;
    }

    public void OnFire()
    {
        if (playerStats.currentAmmo > 0 && !pause.isPaused && canMove)
        {
            
            rangedAttack.Fire();
        }
        
    }

    public void OnReload()
    {
        if (!isDashing && canAttack && !pause.isPaused)
        {
            rangedAttack.reloadTrigger = true;
        }
    }

    public void OnTab()
    {
        if (!pause.isPaused && !shopUI.isEnabled)
        {
            FindObjectOfType<InventoryManager>(true).isEnabled = !FindObjectOfType<InventoryManager>(true).isEnabled;
            FindObjectOfType<InventoryManager>(true).Update();
        }
        if (FindObjectOfType<InventoryManager>(true).isEnabled)
        {
            FindObjectOfType<InventoryManager>(true).GetComponent<Fade>().ShowUI();
        }
        else
        {
            FindObjectOfType<InventoryManager>(true).GetComponent<Fade>().HideUI();
        }
       
    }

    public void FlipBack()
    {
        spriteRenderer.flipX = GetComponentInChildren<RangedAttack>().facingLeft;
    }

}
