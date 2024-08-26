using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : EnemyDamage
{
    [Header("Move Point")]
    [SerializeField] public Transform pointA; // Điểm A
    [SerializeField] public Transform pointB; // Điểm B
    [SerializeField] public float speed; // Tốc độ di chuyển

    [Header("Check")]
    private bool movingToB = true; // Trạng thái di chuyển
    private bool isWaiting = false; // Trạng thái doi

    [Header("Refrence")]
    private Animator amin;
    private Vector3 localSacle;
    private Health playerHealth;

    [Header("Sight Attack")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float boxDistance;
    [SerializeField] private float range;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform player;

    [Header("Attack")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private float bulletRate; //speed vien dan
    private float nextBulletTime;


    private void Start()
    {
        amin = GetComponent<Animator>();
        localSacle = transform.localScale; // luu lai scale ban dau
    }

    private void Update()
    {
        if (!isWaiting)
        {
            if (PlayerInSight())
            {
                Attack();
            }
            else
            {
                Move();
            }
        }
    }

    private void Move()
    {
        amin.SetTrigger("run");

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

    private void Attack()
    {
        if (Time.time >= nextBulletTime)
        {
            amin.SetTrigger("attack");

            // tạo viên đạn từ bulletPrefab tại vị trí firePoint
            GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation); 

            // // Lấy hướng bắn từ hướng của kẻ địch
            Vector2 shootDirection = transform.localScale.x > 0 ? Vector2.left : Vector2.right;

            // Kich hoat vien dan va tao huong ban
            bullet.GetComponent<BulletTrunk>().ActivateProjectile(shootDirection);

            // Ví dụ minh họa
            // Nếu Time.time là 10 giây và fireRate là 2(tức là 2 viên đạn mỗi giây)
            // nextFireTime = 10 + 1f / 2; // nextFireTime = 10 + 0.5; // nextFireTime = 10.5
            nextBulletTime = Time.time + 1f / bulletRate; 
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
    }
}

