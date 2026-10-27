using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialogueHolder; 

    [Header("Player")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Rigidbody2D playerRb;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;
        if (collision.gameObject.name.Equals("Fawn"))
        {
            triggered = true;
            StartCoroutine(PlayDialogue());
        }
    }

    private IEnumerator PlayDialogue()
    {
        if (player != null)
        {
            player.enabled = false;
            if (playerRb != null) playerRb.linearVelocity = Vector2.zero;
        }

        dialogueHolder.SetActive(true);

        yield return new WaitUntil(() => !dialogueHolder.activeSelf);

        if (player != null) player.enabled = true;
    }
}