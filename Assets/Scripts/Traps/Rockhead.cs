using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rockhead : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float rangge;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    private Vector3[] direction = new Vector3[4];
    private Vector3 destination;
    private float checkTimer;
    private bool attacking;
    private bool shouldFall;
    private Animator amin;
    private Rigidbody2D rb;


    private void Start()
    {
        amin = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        Stop();
    }
    private void Update()
    {
        if (attacking)
        {
            transform.Translate(destination * speed * Time.deltaTime);
        }
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
            {
                CheckForPlayer();
            }

            if (shouldFall)
            {
                rb.velocity = new Vector2(0, -speed);
            }
        }
    }

    private void CheckForPlayer()
    {
        CalculateDirection();
         for (int i = 0; i < direction.Length; i++)
        {
            Debug.DrawRay(transform.position, direction[i], Color.red);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction[i], rangge, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = direction[i];
                checkTimer = 0;
            }
        }
    }

    private void CalculateDirection()
    {
        direction[0] = transform.right * rangge;
        direction[1] = -transform.right * rangge;
        direction[2] = transform.up * rangge;
        direction[3] = -transform.up * rangge;
    }

    private void Stop()
    {
        destination = Vector3.zero;
        attacking = false;
        shouldFall = false;
        rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player"))
        {
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            Vector2 directionRock = contactPoint - (Vector2)transform.forward;

            if (Mathf.Abs(directionRock.x) > Mathf.Abs(directionRock.y))
            {
                if (directionRock.x > 0)
                {
                    amin.SetTrigger("HitRight");
                }
                else
                {
                    amin.SetTrigger("HitLeft");
                }
            }
            else
            {
                if (directionRock.y < 0)
                {
                    amin.SetTrigger("HitBottom");
                    shouldFall = true;
                }
            }
        }
        Stop();
    }
}
