using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ItemCollect : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSFX;

    [SerializeField] private GameObject promptObject;

    [Header("Inventory")]
    [SerializeField] private GameObject inventoryItemPrefab;

    [Header("Popup")]
    [SerializeField] private string itemName = "Item";
    [SerializeField] private Sprite itemIcon;

    [Header("One-line dialogue (optional)")]
    [SerializeField] private GameObject dialogueHolder;

    [Header("Player")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Animator playerAnimator;           // <-- new
    [SerializeField] private string walkBool = "IsWalking";     // <-- new

    [Header("Persistence")]
    [SerializeField] private string pickupId;

    [Header("Events")]
    public UnityEvent onPickupSequenceFinished;   // <-- new

    

    private bool pickUpAllowed;
    private bool collecting;
    private bool playerInside;

    private void Start()
    {
        if (!string.IsNullOrEmpty(pickupId) && CollectedPickups.IsCollected(pickupId))
        {
            if (promptObject != null) promptObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        promptObject.SetActive(false);
    }

    private void Update()
    {
        bool playerFree = player != null && player.enabled && player.canMove;
        bool canInteract = playerInside && playerFree && !collecting;

        pickUpAllowed = canInteract;
        if (promptObject != null && promptObject.activeSelf != canInteract)
            promptObject.SetActive(canInteract);

        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E))
            StartCoroutine(CollectSequence());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Fawn"))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Fawn"))
            playerInside = false;
    }

    private IEnumerator CollectSequence()
    {
        if (!InsideInventoryController.instance.AddItem(inventoryItemPrefab))
        {
            Debug.Log("Inventory full");
            yield break;
        }

        SoundManager.instance.PlaySound(pickupSFX);

        collecting = true;
        pickUpAllowed = false;
        promptObject.SetActive(false);

        if (!string.IsNullOrEmpty(pickupId))
        {
            CollectedPickups.MarkCollected(pickupId);
        }

        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
            sr.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        player.enabled = false;
        playerRb.linearVelocity = Vector2.zero;
        if (playerAnimator != null) playerAnimator.SetBool(walkBool, false);   // <-- new

        ItemPopupUI.instance.Show(itemIcon, itemName);

        if (dialogueHolder != null)
        {
            dialogueHolder.SetActive(true);
            yield return new WaitUntil(() => !dialogueHolder.activeSelf);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        ItemPopupUI.instance.Hide();
        player.enabled = true;
        promptObject.SetActive(false);

        onPickupSequenceFinished?.Invoke();

        Destroy(gameObject);
    }
}