using System.Collections;
using UnityEngine;

public class Level3Intro : MonoBehaviour
{
    [Header("Cutscene")]
    [SerializeField] private GameObject cutsceneHolder;   // the CutsceneDialogueHolder that plays on entry

    [Header("Fawn")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private string walkBool = "IsWalking";

    [Header("Buck")]
    [SerializeField] private BuckChase buck;
    [SerializeField] private Transform spawnPoint;        // optional, leave empty to use Buck's current position
    [SerializeField] private float graceSeconds = 1f;     // head start after he appears

    private IEnumerator Start()
    {
        // Freeze Fawn immediately
        player.enabled = false;
        playerRb.linearVelocity = Vector2.zero;
        if (playerAnimator != null) playerAnimator.SetBool(walkBool, false);

        // Wait for the cutscene to start, then finish
        if (cutsceneHolder != null)
        {
            yield return null;
            yield return new WaitUntil(() => !cutsceneHolder.activeSelf);
        }

        // Buck appears
        if (spawnPoint != null) buck.transform.position = spawnPoint.position;
        buck.gameObject.SetActive(true);

        // Fawn can move again, and the chase begins
        player.enabled = true;
        buck.StartChaseWithDelay(graceSeconds);
    }
}