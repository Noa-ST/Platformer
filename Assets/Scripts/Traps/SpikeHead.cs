using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeHead : EnemyDamage
{
    [Header("SpikeHead Atributes")]
    [SerializeField] private float speed; // tốc độ di chuyển
    [SerializeField] private float range; // phạm vi phát hiện
    [SerializeField] private float checkDelay; // thời gian chờ giữa các lần kiểm tra
    [SerializeField] private LayerMask playerLayer; // lớp của người chơi (để phát hiện người chơi)
    private Vector3[] directions = new Vector3[4]; //mảng các hướng (trái, phải, lên, xuống) để kiểm tra
    private Vector3 destination; // vị trí đích đến của SpikeHead
    private float checkTimer; // trì hoãn việc kiểm tra lại của SpikeHead sau một khoảng thời gian nhất định.
    private bool attacking; // trạng thái SpikeHead đang tấn công hay không
    private bool shouldFall; //Kiem tra trang thai roi cxuong mat dat chuwa

    private void OnEnable()
    {
        // Khi SpikeHead được kích hoạt, phương thức này sẽ được gọi.
        Stop();
    }

    private void Update()
    {
        if (attacking)
            // Di chuyển đến vị trí đích đến của SpikeHead với tốc độ
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            checkTimer += Time.deltaTime;
            // Kiểm tra xem đã đến thời gian chờ hay chưa
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }

        if (shouldFall)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }

    private void CheckForPlayer()
    // Phương thức này kiểm tra xem có người chơi trong phạm vi phát hiện (range) hay không.
    {
        CalculateDirections();

        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            // Dòng mã này phát hiện một tia 2D từ vị trí của SpikeHead trong một trong bốn hướng (trái, phải, lên, xuống).
            // Nó kiểm tra xem tia có va chạm với một đối tượng trên lớp playerLayer trong khoảng cách range hay không.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                // Đặt đích đến của SpikeHead là hướng hiện tại (directions[i]) mà nó đã phát hiện ra người chơi.
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }

    private void CalculateDirections()
    {
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range;
        directions[2] = transform.up * range;
        directions[3] = -transform.up * range;
    }

    private void Stop()
    {
        //Phương thức này dừng SpikeHead lại. Nó đặt vị trí đích đến (destination) là vị trí hiện tại và đặt trạng thái tấn công (attacking) là false.
        destination = transform.position;
        attacking = false;
        shouldFall = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        shouldFall = true;

        Stop();
    }
}

