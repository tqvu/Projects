using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth;
    public int currentHealth;
    public int maxAmmo;
    public int currentAmmo;
    public int currency;
    public int attackDamage;

    public List<ShopItems> smallPowerups;
    public List<Powerup> largePowerups;

    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDeath;
    public static event Action OnPlayerHeal;

    [Header("Damage Frames")]
    public bool hurt = false;
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int numberOfFlashes;

    [Header("Borrowed Componments")]
    [SerializeField] private SpriteRenderer spriteRend;
    public GameObject UIRender;
    public Animator animator;
    public PlayerControls playerControls;
    public LevelLoader loader;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRend = GetComponent<SpriteRenderer>();
        currentAmmo = maxAmmo;
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            largePowerups.Clear();
        }
        attackDamage = GetComponentInChildren<MeleeController>(true).attackDamage;
        if(currentHealth <= 2 && currentHealth != 0)
        {
            FindObjectOfType<Volume>().GetComponent<Volume>().enabled = true;
        }
        else
        {
            FindObjectOfType<Volume>().GetComponent<Volume>().enabled = false;
        }
    }

    public void Save()
    {
        currency = FindObjectOfType<CurrencyManager>(true).currency;
        SaveManager.SavePlayer(gameObject.GetComponent<PlayerStats>());
    }

    public void Load()
    {
        PlayerData data = SaveManager.LoadPlayer();
        currency = data.currency;
        FindObjectOfType<CurrencyManager>(true).currency = currency;
        attackDamage = data.attackDamage;
        maxHealth = data.maxHealth;
        maxAmmo = data.maxAmmo;

        for(int i = 0; i < smallPowerups.Count; i++)
        {
            if (smallPowerups[i].ID == data.powerups[i].ID)//check if there is matching ID in save data to inventory
            {
                smallPowerups[i].level = data.powerups[i].level;
                smallPowerups[i].enabled = data.powerups[i].enabled;
            }
        }
    }

    public void AddPowerup(Powerup p)
    {
        largePowerups.Add(p);
    }

    public void DamageTaken(int amount)
    {
        currentHealth -= amount;
        OnPlayerDamaged?.Invoke();
        StartCoroutine(Invulnerabilty());

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnPlayerDeath?.Invoke();
            playerControls.canMove = false;
            playerControls.canDash = false;
            playerControls.isMoving = false;
            playerControls.isDeath = true;
            foreach(Sound sound in FindObjectOfType<AudioManager>().sounds)
            {
                sound.source.Stop();
            }

            Canvas[] canvases = FindObjectsOfType<Canvas>(true);
            foreach(Canvas c in canvases)
            {
                if (c.CompareTag("Respawn"))
                {
                    continue;
                }
                c.gameObject.SetActive(false);
            }
            FindObjectOfType<AudioManager>().Stop("F1 BGM");
            FindObjectOfType<AudioManager>().Stop("F2 BGM");

            SceneManager.LoadScene("Death");
            animator.SetTrigger("Death");
            //move to death scene
            //turn off everything except player
            //play animation after transition to scene
            //fade to hub
            //fade out or game over scene
        }
    }

    IEnumerator Respawn()
    {
        loader = FindObjectOfType<LevelLoader>();
        foreach(AudioSource s in FindObjectOfType<AudioManager>().GetComponents<AudioSource>())
        {
            s.Stop();
        }
        yield return StartCoroutine(loader.LoadingLevel("Hub"));
        animator.SetTrigger("Respawn");


        Canvas[] canvases = FindObjectsOfType<Canvas>(true);
        foreach (Canvas c in canvases)
        {
            c.gameObject.SetActive(true);
        }
        foreach(Powerup p in largePowerups)
        {
            p.Deactivate(gameObject);
        }
        largePowerups.Clear();
        currentHealth = maxHealth;
        playerControls.canMove = true;
        playerControls.canDash = true;
        playerControls.isMoving = true;
        playerControls.isDeath = false;
        playerControls.floor_number = 1;
        playerControls.usedScenes.Clear();
    }
    public void Healing(int amount)
    {
        currentHealth += amount;
        OnPlayerHeal?.Invoke();

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    
    private IEnumerator Invulnerabilty()
    {
        hurt = true;
        FindObjectOfType<AudioManager>().Play("Player Hurt");
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0,0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));

        }
        hurt = false;
    }
}