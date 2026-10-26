using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public float moveSpeed;
    public float input;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");

        if (input < 0) {
            spriteRenderer.flipX = false;
        } else if (input > 0) {
            spriteRenderer.flipX = true;
        }

        animator.SetBool("IsWalking", input != 0);
    }

    void FixedUpdate()
    {
        playerRb.linearVelocity = new Vector2(input * moveSpeed, playerRb.linearVelocity.y);
    }
}