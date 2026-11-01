using System.Collections;
using UnityEngine;

public class BuckChase : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;            // Fawn
    [SerializeField] private PlayerMovement player;       // her speed + freezing her when caught
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Start mode")]
    [SerializeField] private bool startOnSceneLoad = true;   // ON in Level3, OFF in LockedRoom / Level2

    [Header("Cutscene")]
    [SerializeField] private GameObject cutsceneHolder;   // object with DialogueHolder; chase starts when it closes

    [Header("Chase")]
    [SerializeField] private float startDelay = 1f;
    [Range(0.5f, 1f)]
    [SerializeField] private float percentOfRunSpeed = 0.92f; // slightly slower than Fawn running

    [Header("Pass-through (LockedRoom / Level2)")]
    [SerializeField] private bool passThroughPlayer = false;  // leave OFF in Level3

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
    private Coroutine startRoutine;

    private float ChaseSpeed => player.moveSpeed * player.runMultiplier * percentOfRunSpeed;

    private void Start()
    {
        rb.freezeRotation = true;
        transform.rotation = Quaternion.identity;

        if (startOnSceneLoad) StartCoroutine(BeginChase());
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

    // Used by the pickup event in the Inspector
    public void StartChase()
    {
        StartChaseWithDelay(startDelay);
    }

    // Used by code that wants its own head start (e.g. BuckChaseResume in Level2)
    public void StartChaseWithDelay(float delay)
    {
        if (chasing || caught || startRoutine != null) return;
        GameProgress.BuckChasing = true;
        startRoutine = StartCoroutine(StartChaseRoutine(delay));
    }

    private IEnumerator StartChaseRoutine(float delay)
    {
        rb.simulated = true;
        rb.freezeRotation = true;

        if (passThroughPlayer)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;     
            var col = GetComponent<Collider2D>();
            if (col != null) col.isTrigger = true;        // Fawn walks through him
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        yield return new WaitForSeconds(delay);
        chasing = true;
        startRoutine = null;
    }

    // Call from your door / GoodEnding trigger
    public void StopChase()
    {
        if (startRoutine != null)
        {
            StopCoroutine(startRoutine);
            startRoutine = null;
        }

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

    // Solid collider catch (Level3)
    private void OnCollisionEnter2D(Collision2D collision) => TryCatch(collision.gameObject);
    private void OnCollisionStay2D(Collision2D collision) => TryCatch(collision.gameObject);

    // Trigger catch (pass-through mode)
    private void OnTriggerEnter2D(Collider2D other) => TryCatch(other.gameObject);
    private void OnTriggerStay2D(Collider2D other) => TryCatch(other.gameObject);

    private void TryCatch(GameObject other)
    {
        if (!chasing || caught) return;
        if (other.name.Equals("Fawn"))
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