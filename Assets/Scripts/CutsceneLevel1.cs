using System.Collections;
using UnityEngine;

public class CutsceneLevel1 : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Transform npc;
    [SerializeField] private Animator npcAnimator;
    [SerializeField] private SpriteRenderer npcSprite;
    [SerializeField] private AudioSource npcAudio;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private bool npcFacesRightByDefault = false;
    [SerializeField] private GameObject dialogueHolder;

    private void Start()
    {
        StartCoroutine(RunCutscene());
    }

    private IEnumerator RunCutscene()
    {
        player.enabled = false;
        playerRb.linearVelocity = Vector2.zero;

        dialogueHolder.SetActive(true);
        yield return new WaitUntil(() => !dialogueHolder.activeSelf);

        bool movingRight = exitPoint.position.x > npc.position.x;
        npcSprite.flipX = npcFacesRightByDefault ? !movingRight : movingRight;
        npcAnimator.SetBool("IsWalking", true);
        npcAudio.Play();

        while (Mathf.Abs(npc.position.x - exitPoint.position.x) > 0.05f)
        {
            float newX = Mathf.MoveTowards(
                npc.position.x, exitPoint.position.x, walkSpeed * Time.deltaTime);
            npc.position = new Vector3(newX, npc.position.y, npc.position.z);
            yield return null;
        }

        npcAudio.Stop();
        npcAnimator.SetBool("IsWalking", false);
        npc.gameObject.SetActive(false);

        player.enabled = true;
    }
}