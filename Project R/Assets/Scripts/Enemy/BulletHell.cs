using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHell : MonoBehaviour
{
    public GameObject fireballPrefab;
    public int segments;


    //shoot at them after animation
    //random spawn point for this object  //ave list of possible spawns to shoot at player
    private void Awake()
    {
        //play animation
    }

    public void Execute()
    {
        StartCoroutine(Loop());
    }

    public void Shoot()
    {
        float angleStep = 360f / segments;
        float angle = 0f;
        for (int i = 1; i <= segments; i++)
        {
            float projDirX = transform.position.x + Mathf.Sin(angle * Mathf.PI / 180);
            float projDirY = transform.position.y + Mathf.Cos(angle * Mathf.PI / 180);

            Vector3 projectileVector = new Vector3(projDirX, projDirY, 0);
            Vector3 moveDirection = (projectileVector - transform.position).normalized;

            GameObject bullet = Instantiate(fireballPrefab, transform.position, Quaternion.AngleAxis(-angle, transform.forward) * transform.rotation);
            bullet.GetComponentInChildren<Rigidbody2D>().AddForce(moveDirection * 3, ForceMode2D.Impulse);
            angle += angleStep;
            Destroy(bullet, 2);
        }
        
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }
    
    public IEnumerator Loop()
    {
        for(int i = 0; i < 5; i++)
        {
            Shoot();
            yield return new WaitForSeconds(.2f);
            
        }
        Destroy();
        
    }
}
