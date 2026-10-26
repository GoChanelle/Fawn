using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public float moveSpeed;
    public float input;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public AudioSource walkAudio;

    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");

        if (input < 0) {
            spriteRenderer.flipX = false;
        } else if (input > 0) {
            spriteRenderer.flipX = true;
        }

        bool isWalking = input != 0;
        animator.SetBool("IsWalking", isWalking);

        if (isWalking && !walkAudio.isPlaying)
            walkAudio.Play();
        else if (!isWalking && walkAudio.isPlaying)
            walkAudio.Stop();
    }

    void FixedUpdate()
    {
        playerRb.linearVelocity = new Vector2(input * moveSpeed, playerRb.linearVelocity.y);
    }
}