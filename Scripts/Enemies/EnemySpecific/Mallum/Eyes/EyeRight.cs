using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeRight : Eye
{
    private float startTime = 0;
    private float nextLazer = 2;
    private float nextLazerPeriod = 2;

    private bool stopMoving = false;
    private bool shooting = false;
    private bool hitOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        sr.enabled = false;

        smallLazer = transform.Find("SmallLazer").gameObject;
        bigLazer = transform.Find("BigLazer").gameObject;

        smallLazer.SetActive(false);
        bigLazer.SetActive(false);
        //this.gameObject.SetActive(false);

        movementSpeed = 3;
        minAxis = -4.5f;
        maxAxis = 10.3f;

        attackDetails.damageAmount = 10;
        attackDetails.position = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.time;
        if (time > nextLazer && !shooting)
        {
            startTime = time;

            stopMoving = true;
            sr.enabled = true;
            shooting = true;

            smallLazer.SetActive(true);
        }
        if (time > startTime + 1 && shooting)
        {
            bigLazer.SetActive(true);
            HitPlayer();

        }
        if (time > startTime + 1.2 && shooting)
        {
            stopMoving = false;
            shooting = false;
            sr.enabled = false;

            smallLazer.SetActive(false);
            bigLazer.SetActive(false);

            nextLazer = time + nextLazerPeriod;
            hitOnce = false;
        }

        attackDetails.position = rb.position;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HitPlayer()
    {
        Vector2 point = new Vector2(rb.position.x, rb.position.y);
        Vector2 size = new Vector2(200, 1.25f);

        Collider2D damageHit = Physics2D.OverlapBox(point, size, 0, whatIsPlayer);

        if (damageHit && !hitOnce)
        {
            damageHit.transform.SendMessage("Damage", attackDetails);
            hitOnce = true;
        }

    }

    private void ApplyMovement()
    {
        if (!stopMoving)
        {
            if (rb.position.y <= minAxis)
            {
                movementSpeed = -movementSpeed;
            }
            if (rb.position.y >= maxAxis)
            {
                movementSpeed = -movementSpeed;
            }

            rb.velocity = new Vector2(0, movementSpeed);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 point = new Vector3(rb.position.x + 20, rb.position.y, 1);
        Vector3 size = new Vector3(50, 1.25f, 1);
        Gizmos.DrawWireCube(point, size);
    }
}
