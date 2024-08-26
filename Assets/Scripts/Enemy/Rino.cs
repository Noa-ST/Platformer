using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rino : EnemyDamage
{
    [Header("Move Point")]
    [SerializeField] public Transform pointA; // Điểm A
    [SerializeField] public Transform pointB; // Điểm B
    [SerializeField] public float speed; // Tốc độ di chuyển
    [SerializeField] public float chaseSpeed; // Tốc độ duoi theo ng chs

    [Header("Check")]
    private bool movingToB = true; // Trạng thái di chuyển
    private bool isWaiting = false; // Trạng thái doi
    private bool ischaseSpeed = false; // Trạng thái duoitheo ng chs

    [Header("Refrence")]
    private Animator amin;
    private Rigidbody2D rb;
    private Vector3 localSacle;
    private Health playerHealth;

    [Header("Attack")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float boxDistance;
    [SerializeField] private float range;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform player;

    private void Start()
    {
        amin = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        localSacle = transform.localScale; // luu lai scale ban dau
    }

    private void Update()
    {
        if (!isWaiting)
        {
            if (PlayerInSight())
            {
                ischaseSpeed = true;
                ChasePlayer();
            }
            else
            {
                ischaseSpeed = false;
                Move();
            }
        }
    }

    private void Move()
    {
        amin.SetTrigger("walk");

        if (movingToB)
        {
            // Di chuyển từ điểm A đến điểm B
            transform.position = Vector2.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

            // Kiểm tra nếu đã đến điểm B
            if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
            {
                movingToB = false;
                StartCoroutine(Waiting());
            }
            FlipX(pointB.position);
        }
        else
        {
            // Di chuyển từ điểm B đến điểm A
            transform.position = Vector2.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);

            // Kiểm tra nếu đã đến điểm A
            if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
            {
                movingToB = true;
                StartCoroutine(Waiting());
            }
            FlipX(pointA.position);
        }
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            FlipX(player.position);
        }
    }

    private void FlipX(Vector3 targetPosion)
    {
        if (transform.position.x > targetPosion.x)
        {
            localSacle.x = Mathf.Abs(localSacle.x); // quay sang phai
        }
        else
        {
            localSacle.x = -Mathf.Abs(localSacle.x); // sang trai
        }
        transform.localScale = localSacle; // Cập nhật scale
    }

    private IEnumerator Waiting()
    {
        isWaiting = true;
        yield return new WaitForSeconds(3f);
        isWaiting = false;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * boxDistance,
                                              new UnityEngine.Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y,
                                               boxCollider.bounds.size.z), 0, UnityEngine.Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * boxDistance,
                           new UnityEngine.Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        // Thêm hành vi đặc biệt cho AngryPig nếu cần
    }
}
