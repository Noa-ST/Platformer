using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed; // Tốc độ di chuyển của viên đạn.
    private float direction; // Hướng di chuyển của viên đạn.
    private bool hit; // Kiểm tra xem viên đạn đã va chạm với vật thể khác hay chưa.
    private float lifetime; // Thời gian sống của viên đạn.

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;

        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0); // Di chuyển viên đạn theo trục x.
        lifetime += Time.deltaTime;
        
        if (lifetime > 5)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        hit = true;
        boxCollider.enabled = false; // Vô hiệu hóa BoxCollider2D của viên đạn.
        anim.SetTrigger("Hit");

        if (col.tag == "Enemy")
            col.GetComponent<Health>().TakeDamage(1);
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        lifetime = 0; // Đặt thời gian sống của viên đạn về 0.
        gameObject.SetActive(true); // Kích hoạt đối tượng viên đạn.
        hit = false;
        boxCollider.enabled = true; // Kích hoạt lại BoxCollider2D của viên đạn.

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false); // Vô hiệu hóa viên đạn.
    }
}
