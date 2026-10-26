using UnityEngine;

public class ItemCollect : MonoBehaviour
{
    [SerializeField] private GameObject promptObject;
    [SerializeField] private GameObject inventoryItemPrefab;

    private bool pickUpAllowed;

    private void Start()
    {
        promptObject.SetActive(false);
    }

    private void Update()
    {
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E))
            PickUp();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Fawn"))
        {
            promptObject.SetActive(true);
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Fawn"))
        {
            promptObject.SetActive(false);
            pickUpAllowed = false;
        }
    }

    private void PickUp()
{
    if (InsideInventoryController.instance.AddItem(inventoryItemPrefab))
    {
        Destroy(gameObject);
    }
    else
    {
        Debug.Log("Inventory full");
    }
}
}