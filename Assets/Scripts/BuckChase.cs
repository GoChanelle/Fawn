using System.Collections;
using UnityEngine;

public class BuckChase : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;            // Fawn
    [SerializeField] private PlayerMovement player;       // her speed + freezing her when caught
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Cutscene")]
    [SerializeField] private GameObject cutsceneHolder;   // object with DialogueHolder; chase starts when it closes

    [Header("Chase")]
    [SerializeField] private float startDelay = 1f;
    [Range(0.5f, 1f)]
    [SerializeField] private float percentOfRunSpeed = 0.92f; // slightly slower than Fawn running

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private string walkBool = "IsWalking";
    [SerializeField] private bool spriteFacesLeft = true;

    [Header("Bad ending")]
    [SerializeField] private string badEndScene = "BadEnd";

    private bool chasing;
    private bool caught;

    private float ChaseSpeed => player.moveSpeed * player.runMultiplier * percentOfRunSpeed;

    private void Start()
    {
        rb.freezeRotation = true;
        transform.rotation = Quaternion.identity;

        StartCoroutine(BeginChase());
    }

    private IEnumerator BeginChase()
    {
        if (cutsceneHolder != null)
        {
            yield return null; // let the holder activate and start its dialogue
            yield return new WaitUntil(() => !cutsceneHolder.activeSelf);
        }

        yield return new WaitForSeconds(startDelay);
        chasing = true;
    }

    // Call from your GoodEnding trigger
    public void StopChase()
    {
        chasing = false;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        SetWalking(false);
    }

    private void FixedUpdate()
    {
        if (!chasing || caught) return;

        float dir = Mathf.Sign(target.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * ChaseSpeed, rb.linearVelocity.y);

        spriteRenderer.flipX = spriteFacesLeft ? dir > 0 : dir < 0;
        SetWalking(true);
    }

    private void OnCollisionEnter2D(Collision2D collision) => TryCatch(collision);
    private void OnCollisionStay2D(Collision2D collision) => TryCatch(collision);

    private void TryCatch(Collision2D collision)
    {
        if (!chasing || caught) return;
        if (collision.gameObject.name.Equals("Fawn"))
            StartCoroutine(CaughtSequence());
    }

    private IEnumerator CaughtSequence()
    {
        caught = true;
        chasing = false;

        rb.linearVelocity = Vector2.zero;
        SetWalking(false);

        player.enabled = false;
        playerRb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);
        LevelManager.Instance.LoadScene(badEndScene, "CrossFade");
    }

    private void SetWalking(bool value)
    {
        if (animator != null && !string.IsNullOrEmpty(walkBool))
            animator.SetBool(walkBool, value);
    }
}