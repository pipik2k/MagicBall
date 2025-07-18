using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float baseJumpForce = 8f;
    public float gravityScaleStart = 1f;
    public float gravityIncreasePerBounce = 0.5f;
    public float horizontalSpeed = 3f;

    private Rigidbody2D rb;
    private float currentGravityScale;
    private int horizontalDirection = 1; // 1 = phải, -1 = trái
    public float jumpHeight = 3;
    private BallSound ballSound;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentGravityScale = gravityScaleStart;
        rb.gravityScale = currentGravityScale;
        ballSound = GetComponent<BallSound>();
        // Gán vận tốc ngang ban đầu
        rb.velocity = new Vector2(horizontalSpeed * horizontalDirection, 0);
    }

    void FixedUpdate()
    {
        // Cập nhật vận tốc ngang giữ nguyên liên tục
        rb.velocity = new Vector2(horizontalSpeed * horizontalDirection, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BoxCollider boxCollider = collision.gameObject.GetComponent<BoxCollider>();
        Vector2 normal = collision.contacts[0].normal;

        if (normal.y > 0.9f)
        {
            ballSound.PlaySoundEffect();
        }

        if (Mathf.Abs(normal.x) > 0.9f)
        {
            horizontalDirection *= -1;
        }

        if (boxCollider)
        {
            boxCollider.TakeDamage();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        if (normal.y > 0.9f)
        {
            currentGravityScale += gravityIncreasePerBounce;
            rb.gravityScale = currentGravityScale;
            var jumpHeightRate = 0.2f;
            var smallestJumpHeight = 0.4f;
            jumpHeight -= jumpHeightRate;
            jumpHeight = Mathf.Max(jumpHeight, smallestJumpHeight);
            float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
            float minRequiredJump = Mathf.Sqrt(2 * gravity * smallestJumpHeight);
            float requiredJump = Mathf.Sqrt(2 * gravity * jumpHeight);
            requiredJump = Mathf.Max(requiredJump, minRequiredJump);

            rb.velocity = new Vector2(rb.velocity.x, requiredJump);
        }
    }
}
