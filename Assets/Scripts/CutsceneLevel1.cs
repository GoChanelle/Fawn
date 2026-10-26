using System.Collections;
using UnityEngine;

public class CutsceneLevel1 : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Transform npc;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private float walkSpeed = 2f;
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

        while (Vector2.Distance(npc.position, exitPoint.position) > 0.05f)
        {
            npc.position = Vector2.MoveTowards(
                npc.position, exitPoint.position, walkSpeed * Time.deltaTime);
            yield return null;
        }
        npc.gameObject.SetActive(false);

        player.enabled = true;
    }
}
