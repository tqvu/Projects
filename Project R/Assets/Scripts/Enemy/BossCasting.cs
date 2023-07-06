using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCasting : MonoBehaviour
{
    public List<GameObject> casts;
    private GameObject player;
    int phase = 1;

    public void Cast()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(0.29f, 4.22f, 0);
        switch(Random.Range(0, 11))//random chance to cast certain abilities (will weight soon
        {
            case >= 0 and <= 4:
                //for loop to pick from spawnpoints to do stuff
                Debug.Log("Fireball");
                StartCoroutine(FireRate());
                break;
            case >4 and <= 6:
                for(int i = 0; i < phase; i++)
                {
                    Debug.Log("Laser");
                    Instantiate(casts[1], GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);//laser spawning
                }
                break;
            case > 6 and <= 8:
                for(int i = 0; i < phase; i++)
                {
                    Debug.Log("Bullet Hell");
                    Instantiate(casts[2], (Vector2)player.transform.position + (Random.insideUnitCircle), Quaternion.identity);//spread fireball spawning
                }
                break;
            case >8 and <= 9:
                for(int i = 0; i < phase; i++)
                {
                    Debug.Log("Adds");
                    Instantiate(casts[3], (Vector2)player.transform.position + (Random.insideUnitCircle), Quaternion.identity);//adds spawning
                }
                break;
            case > 9 and <= 10:
                for (int i = 0; i < phase; i++)
                {
                    Debug.Log("Adds");
                    Instantiate(casts[4], (Vector2)player.transform.position + (Random.insideUnitCircle), Quaternion.identity);//adds spawning
                }
                break;
            default:
                break;
        }
    }

    public void PlaySound()
    {
        FindObjectOfType<AudioManager>().Play("Boss Cast");
    }

    public IEnumerator FireRate()
    {
        List<GameObject> fireballs = new List<GameObject>();
        for (int i = 0; i < 3 * phase; i ++)
        {
            fireballs.Add(Instantiate(casts[0], (Vector2)player.transform.position + (Random.insideUnitCircle), Quaternion.identity));//fireball cast)
            FindObjectOfType<AudioManager>().Play("Boss Fireball");
            yield return new WaitForSeconds(.2f);
        }

    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position, 1);
    }
}
