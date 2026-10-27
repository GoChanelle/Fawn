using System.Collections;
using UnityEngine;

public class ItemCollect : MonoBehaviour
{
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

    [Header("Persistence")]
    [SerializeField] private string pickupId;

    private bool pickUpAllowed;
    private bool collecting;

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
        if (pickUpAllowed && !collecting && Input.GetKeyDown(KeyCode.E))
            StartCoroutine(CollectSequence());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collecting) return;
        if (collision.gameObject.name.Equals("Fawn"))
        {
            promptObject.SetActive(true);
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collecting) return;
        if (collision.gameObject.name.Equals("Fawn"))
        {
            promptObject.SetActive(false);
            pickUpAllowed = false;
        }
    }

    private IEnumerator CollectSequence()
    {
        if (!InsideInventoryController.instance.AddItem(inventoryItemPrefab))
        {
            Debug.Log("Inventory full");
            yield break;
        }

        collecting = true;
        pickUpAllowed = false;
        promptObject.SetActive(false);

        if (!string.IsNullOrEmpty(pickupId))
        {
            CollectedPickups.MarkCollected(pickupId);

        }

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        player.enabled = false;
        playerRb.linearVelocity = Vector2.zero;

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
        Destroy(gameObject);
    }
}