using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour 
{
    public bool isInRange;
    public KeyCode[] interactKey;
    public UnityEvent InteractFunction;
    public UnityEvent DeinteractFunction;


    void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(interactKey[0]))
            {
                InteractFunction.Invoke();
            }
            else if (Input.GetKeyDown(interactKey[1]))
            {
                DeinteractFunction.Invoke();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerControls>().PromptEnable();
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerControls>().PromptDisable();
            isInRange = false;
        }
    }
}
