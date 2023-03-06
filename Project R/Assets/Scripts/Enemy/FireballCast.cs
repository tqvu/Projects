using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class FireballCast : MonoBehaviour
{
    public GameObject fireballPrefab;
    Vector2 targetPos;


    //shoot at them after animation
    //random spawn point for this object  //ave list of possible spawns to shoot at player
    public void Shoot()
    {
        targetPos = (Vector2)GameObject.FindGameObjectWithTag("Player").transform.position;
        
        Vector2 difference = targetPos - (Vector2)transform.position;
        float aimAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;//aiming code

        Vector3 moveDirection = difference.normalized;

        GameObject bullet = Instantiate(fireballPrefab, transform.position, Quaternion.AngleAxis(aimAngle - 90f, transform.forward));
        bullet.GetComponentInChildren<Rigidbody2D>().AddForce(moveDirection * 3, ForceMode2D.Impulse);
        Destroy(bullet, 3);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
