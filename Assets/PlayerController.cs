using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpInterval = 0.8f;
    public float jumpForce = 8f;
    public float moveSpeed = 5f;
    public float gravity = 5f;

    public Rigidbody2D rb;
    [SerializeField] private ParticleSystem jumpParticle;

    private bool isGrounded = false;
    private float jumpTimer = 0f;

    void Update()
    {
        // Tilt player visually
        float moveX = Input.GetAxis("Horizontal");
        Quaternion targetRotation = Quaternion.Euler(0, 0, -10f * moveX);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        // Gravity
        rb.velocity += Vector2.down * gravity * Time.fixedDeltaTime;

        // Horizontal movement
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Handle bouncing
        jumpTimer += Time.fixedDeltaTime;
        if (isGrounded && jumpTimer >= jumpInterval)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimer = 0f;
            isGrounded = false;
            SpawnJumpParticles();
        }

        // Optional: Kill weird wall-kick by zeroing X when against wall and no input
        if (IsTouchingWall() && moveX == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    bool IsTouchingWall()
    {
        float wallCheckDistance = 0.05f;
        Vector2 direction = new Vector2(Mathf.Sign(rb.velocity.x), 0f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    void SpawnJumpParticles()
    {
        Vector3 spawnPos = transform.position + Vector3.down * 1f;
        Instantiate(jumpParticle, spawnPos, Quaternion.identity);
    }
}
