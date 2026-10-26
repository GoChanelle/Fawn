using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject promptObject;
    [SerializeField] private GameObject lockedDialogue;

    [Header("Door settings")]
    [SerializeField] private string requiredItemName = "key"; 
    [SerializeField] private string nextSceneName;           
    [SerializeField] private bool consumeKey = true;         

    private bool playerInRange;

    private void Start()
    {
        if (promptObject != null) promptObject.SetActive(false);
        if (lockedDialogue != null) lockedDialogue.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Fawn"))
        {
            playerInRange = true;
            if (promptObject != null) promptObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Fawn"))
        {
            playerInRange = false;
            if (promptObject != null) promptObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange || !Input.GetKeyDown(KeyCode.R)) return;

        if (lockedDialogue != null && lockedDialogue.activeSelf) return;

        if (InsideInventoryController.HasItem(requiredItemName))
        {
            if (consumeKey) InsideInventoryController.ConsumeItem(requiredItemName);
            LevelManager.Instance.LoadScene(nextSceneName, "CrossFade");
        }
        else if (lockedDialogue != null)
        {
            lockedDialogue.SetActive(true); // "It's locked. I need a key."
        }
    }
}