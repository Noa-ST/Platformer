using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallJumpCooldown = 0.2f; // Thời gian chờ giữa các lần nhảy tường
    private bool isGrounded;
    private bool isTouchingWall;
    private bool canDoubleJump;
    private Rigidbody2D rb;
    public CoinManager cm;
    private Animator amin;
    private float horizontalInput;
    private float lastWallJumpTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        amin = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        Move();
        Jump();
        WallJump();
    }

    private void Move()
    {
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
        
        amin.SetBool("isPlayerRun", horizontalInput != 0);
    }

    private void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                canDoubleJump = false;
            }
        }

        amin.SetBool("isPlayerJump", isGrounded==false);
    }

    private void WallJump()
    {
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isTouchingWall && !isGrounded && Time.time > lastWallJumpTime + wallJumpCooldown)
        {
            float wallJumpDirectionX = isTouchingWall ? -Mathf.Sign(transform.localScale.x) : 0;
            rb.velocity = new Vector2(wallJumpDirectionX * speed, jumpForce);
            lastWallJumpTime = Time.time; // Cập nhật thời gian của lần nhảy tường cuối cùng
        }
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded && !isTouchingWall;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            cm.coinCount++;
            Destroy(collision.gameObject);
        }
    }
    private void Start()
    {
        GameManager.instance.MovePlayerToStartPoint(gameObject);
    }
}

