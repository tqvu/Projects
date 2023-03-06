using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBar : MonoBehaviour
{
    public GameObject ammoPreFab;
    public PlayerStats _playerStats;
    public RangedAttack rangedAttack;
    List<Bullet> bullets = new List<Bullet>();
    int Ammo;
    int help = 0;
    // Start is called before the first frame update
    private void OnEnable()
    {
       RangedAttack.OnPlayerFire += DrawBullets;
    }

    private void OnDisable()
    {
        RangedAttack.OnPlayerFire -= DrawBullets;
    }
    //private void OnLevelWasLoaded()
    //{
    //    DrawBullets();
    //}
    void Start()
    {
        rangedAttack = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<RangedAttack>();
        _playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        DrawBullets();
    }

    private void FixedUpdate()
    {
        if(_playerStats == null)
        {
            //_playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }
        DrawBullets();

    }

    public void DrawBullets()
    {
        ClearBullets();
        for(int i = 0; i < _playerStats.currentAmmo; i++)//create empty bullet shell depending on ammo
        {
            CreateBullet();
        }
    }

    public void CreateBullet()
    {
        GameObject newBullet = Instantiate(ammoPreFab);//instantiate prefab
        newBullet.transform.SetParent(transform);//set transform parent

        Bullet newBulletComp = newBullet.GetComponent<Bullet>();//telling component to be empty and update sprite and list accordingly
        bullets.Add(newBulletComp);
    }

    public void removeBullet()
    {
        int size = bullets.Count;
        bullets.Remove(bullets[size - 1]);
        foreach (Transform t in transform)
        {
            help += 1;
            if(_playerStats.currentAmmo < help)
            {
                Destroy(t.gameObject);
            }
        }
        help = 0;

    }

    public void ClearBullets()//destroys everything under the parent object
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        bullets = new List<Bullet>();
    }

}
