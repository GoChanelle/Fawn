using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialogueHolder; 

    [Header("Player")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Choice")]
    [SerializeField] private ChoicePrompt choicePrompt;
    

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
            player.StopMoving();      // add this line: stops animation, audio and input
            player.enabled = false;
            if (playerRb != null) playerRb.linearVelocity = Vector2.zero;
        }

        dialogueHolder.SetActive(true);

        if (choicePrompt != null)
        {
            Transform holder = dialogueHolder.transform;
            DialogueSystem.DialogueLine lastLine =
            holder.GetChild(holder.childCount - 1).GetComponent<DialogueSystem.DialogueLine>();

            yield return new WaitUntil(() => lastLine.typingDone);
            choicePrompt.Show();
            yield return new WaitUntil(() => choicePrompt.finished);

            dialogueHolder.SetActive(false); // close the dialogue box after "continue"
        }
        else
        {
            yield return new WaitUntil(() => !dialogueHolder.activeSelf);
        }

        if (player != null)
        {
            player.enabled = true;
            player.ResumeMoving();    // add this line: StopMoving turned canMove off
        }
    }
}