using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown; // Thời gian chờ giữa các lần tấn công.
    [SerializeField] private Transform firePoint; // Vị trí mà viên đạn lửa sẽ xuất hiện.
    [SerializeField] private GameObject[] fireballs; // Mảng chứa các viên đạn lửa có thể tái sử dụng.
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity; // Bộ đếm thời gian chờ giữa các lần tấn công.

    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack()) // Kiểm tra xem người chơi có nhấp chuột trái và thời gian chờ giữa các lần tấn công đã hết, cũng như nhân vật có thể tấn công không.
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        int fireballIndex = FindFireball();
        Debug.Log("Fireball Index: " + fireballIndex);
        
        if (fireballIndex >= 0 && fireballIndex < fireballs.Length)
        {
            fireballs[fireballIndex].transform.position = firePoint.position;
            fireballs[fireballIndex].GetComponent<Fireball>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
