using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrunk : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Vector2 direction;


    public void ActivateProjectile(Vector2 shootDirection)
    {
        lifetime = 0;
        direction = shootDirection.normalized; // Chuẩn hóa hướng bắn
        gameObject.SetActive(true);
    }

    private void Update()
    {
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(direction * movementSpeed);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        gameObject.SetActive(false);

    }
}
