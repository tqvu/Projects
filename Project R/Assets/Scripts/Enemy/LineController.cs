using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRend;
    public List<Vector3> points;
    void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().enabled = false;
        SetupLines(new Vector3(-5f, transform.position.y, transform.position.z));
        SetPoints();
        StartCoroutine(WindUp());

    }

    public void SetupLines(Vector3 initialPoint)
    {
        lineRend.positionCount = 2;
        points.Add(initialPoint);
        points.Add(new Vector3(initialPoint.x *= -1, initialPoint.y, initialPoint.z));//flips onto the other side of the arena
    }

    public void SetPoints()
    {
        for(int i = 0; i < points.Count; i++)
        {
            lineRend.SetPosition(i, points[i]);
        }
    }

    public IEnumerator WindUp()
    {
        yield return new WaitForSeconds(.75f);
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Animator>().enabled = true;
        FindObjectOfType<AudioManager>().Play("Laser Fire");
    }

    public void EndAttack()
    {
        Destroy(gameObject);
    }
}
