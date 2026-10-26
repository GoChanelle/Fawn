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

    private bool pickUpAllowed;
    private bool collecting;

    private void Start()
    {
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
        // 1. Try to add to the inventory first; stop if it's full
        if (!InsideInventoryController.instance.AddItem(inventoryItemPrefab))
        {
            Debug.Log("Inventory full");
            yield break;
        }

        collecting = true;
        pickUpAllowed = false;
        promptObject.SetActive(false);

        // 2. Hide the item but keep this object alive so the coroutine keeps running
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // 3. Freeze the player
        player.enabled = false;
        playerRb.linearVelocity = Vector2.zero;

        // 4. Show the popup
        ItemPopupUI.instance.Show(itemIcon, itemName);

        // 5. Play the one-line dialogue and wait for it to finish
        if (dialogueHolder != null)
        {
            dialogueHolder.SetActive(true);
            yield return new WaitUntil(() => !dialogueHolder.activeSelf);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        // 6. Clean up
        ItemPopupUI.instance.Hide();
        player.enabled = true;
        Destroy(gameObject);
    }
}