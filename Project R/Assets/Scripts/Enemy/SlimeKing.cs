using System.Collections;
using UnityEngine;

public class SlimeKing : Enemy
{
    public int stage = 1;

    public GameObject bulletPrefab;
    public GameObject shadow;
    private GameObject shadowCircle;
    public float fireForce;
    public bool shadowSpawn = false;
    private float currentTime = 0f;
    private float endTime;

    public enum State
    {
        jupming, landing, shooting, idle, airborne
    }

    public State state;



    // Start is called before the first frame update
    public override void Start()
    {
        state = State.idle;
        base.Start();
    }

    protected override void FixedUpdate()
    {
        switch (state)
        {
            case State.idle:
                base.FixedUpdate();
                break;
            case State.jupming:
                foreach (CircleCollider2D collider in GetComponents<CircleCollider2D>())
                {
                    collider.enabled = false;
                }
                currentTime += Time.fixedDeltaTime;
                Vector3 jumpPosition = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, jumpPosition, currentTime / endTime);
                break;
            case State.airborne:
                currentTime += Time.fixedDeltaTime;
                if (!shadowSpawn)
                {
                    shadowCircle = Instantiate(shadow, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
                    shadowSpawn = true;
                }
                else
                {
                    Debug.Log(shadowCircle.transform.localScale);
                    shadowCircle.transform.localScale = Vector3.Lerp(shadowCircle.transform.localScale, new Vector3(1, 0.5f, 1), currentTime / endTime);//makes the shadow expand 1 second before slime drops
                }
                break;
            case State.landing:
                shadowSpawn = false;
                currentTime += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, new Vector3(shadowCircle.transform.position.x, shadowCircle.transform.position.y + 0.5f, shadowCircle.transform.position.z), currentTime / endTime);
                break;
            
        }
    }

    private void Shoot()
    {
        FindObjectOfType<AudioManager>().Play("Bubble Pop");
        state = State.shooting;
        if (stage == 3)
        {
            float angleStep = 360f / 8;
            float angle = 0f;
            for (int i = 1; i <= 8; i++)
            {
                float projDirX = transform.position.x + Mathf.Sin(angle * Mathf.PI / 180);
                float projDirY = transform.position.y + Mathf.Cos(angle * Mathf.PI / 180);

                Vector3 projectileVector = new Vector3(projDirX, projDirY, 0);
                Vector3 moveDirection = (projectileVector - transform.position).normalized;

                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.AngleAxis(-angle, transform.forward));
                bullet.GetComponentInChildren<Rigidbody2D>().AddForce(moveDirection * 1.5f, ForceMode2D.Impulse);
                angle += angleStep;
                Destroy(bullet, 2);
            }
        }
        else
        {
            Vector3 moveDirection = ((Vector3)pointerInput - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponentInChildren<Rigidbody2D>().AddForce(moveDirection * 1.5f, ForceMode2D.Impulse);
            Destroy(bullet, 2);
        }
    }

    public void Jump()
    {
        FindObjectOfType<AudioManager>().Play("Sword Slash2");
        state = State.jupming;
        currentTime = 0f;
        endTime = .25f;
        StartCoroutine(Land());
    }

    public IEnumerator Land()
    {
        yield return new WaitForSeconds(0.25f);
        state = State.airborne;
        currentTime = 0f;
        endTime = 1f;
        yield return new WaitForSeconds(1f);
        state = State.landing;
        currentTime = 0f;
        endTime = .25f;
        yield return new WaitForSeconds(.25f);
        foreach (CircleCollider2D collider in GetComponents<CircleCollider2D>())
        {
            collider.enabled = true;
        }
        FindObjectOfType<AudioManager>().Play("Slime Jump2");
        yield return new WaitForSeconds(.25f);
        Destroy(shadowCircle);
        state = State.idle;

    }
}
