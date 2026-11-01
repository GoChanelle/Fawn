using System.Collections;
using UnityEngine;

public class TutorialHints : MonoBehaviour
{
    [Header("Hints")]
    [SerializeField] private GameObject sprintHint;      
    [SerializeField] private GameObject inventoryHint;  

    [Header("Wait for the intro cutscene")]
    [SerializeField] private DialogueSystem.DialogueHolder cutsceneHolder;

    [SerializeField] private float delayBetweenHints = 0.5f;
    [SerializeField] private string tutorialId = "Level1_Tutorial";

    private void Start()
    {
        sprintHint.SetActive(false);
        inventoryHint.SetActive(false);

        if (CollectedPickups.IsCollected(tutorialId))
        {
            enabled = false;
            return;
        }

        StartCoroutine(RunTutorial());
    }

    private IEnumerator RunTutorial()
    {
        if (cutsceneHolder != null)
        {
            yield return null; 
            yield return new WaitUntil(() => !cutsceneHolder.gameObject.activeSelf);
        }

        sprintHint.SetActive(true);
        yield return new WaitUntil(() =>
            (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f);
        sprintHint.SetActive(false);

        yield return new WaitForSeconds(delayBetweenHints);

        // Step 2: inventory
        inventoryHint.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Tab));
        inventoryHint.SetActive(false);

        CollectedPickups.MarkCollected(tutorialId);
    }
}