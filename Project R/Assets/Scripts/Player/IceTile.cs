using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            collision.gameObject.GetComponent<PlayerControls>().activeMoveSpeed *= .5f;
            collision.gameObject.GetComponent<Rigidbody2D>().drag = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerControls>().activeMoveSpeed = collision.gameObject.GetComponent<PlayerControls>().baseMoveSpeed;
            collision.gameObject.GetComponent<Rigidbody2D>().drag = 10;
        }
    }
}
