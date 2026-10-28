using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public float moveSpeed;
    public float runMultiplier = 1.6f;
    public float input;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public AudioSource walkAudio;

    public bool canMove = true;

    private bool isRunning;

    public void StopMoving()
    {
        canMove = false;
        input = 0;
        isRunning = false;
        playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y);
        animator.SetBool("IsWalking", false);
        animator.speed = 1f;
        walkAudio.Stop();
    }

    public void ResumeMoving()
    {
        canMove = true;
    }

    void Update()
    {
        if (!canMove) return;

        input = Input.GetAxisRaw("Horizontal");

        if (input < 0) {
            spriteRenderer.flipX = false;
        } else if (input > 0) {
            spriteRenderer.flipX = true;
        }

        bool isWalking = input != 0;
        isRunning = isWalking && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));

        animator.SetBool("IsWalking", isWalking);
        animator.speed = isRunning ? 1.5f : 1f;
        walkAudio.pitch = isRunning ? 1.4f : 1f;

        if (isWalking && !walkAudio.isPlaying)
            walkAudio.Play();
        else if (!isWalking && walkAudio.isPlaying)
            walkAudio.Stop();
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y);
            return;
        }

        float speed = isRunning ? moveSpeed * runMultiplier : moveSpeed;
        playerRb.linearVelocity = new Vector2(input * speed, playerRb.linearVelocity.y);
    }
}