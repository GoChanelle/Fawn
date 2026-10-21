using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCollect : MonoBehaviour
{
    [SerializeField] private TMP_Text pickUpText;

    private bool pickUpAllowed;

    private void Start()
    {
        pickUpText.gameObject.SetActive(false);
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
            pickUpText.gameObject.SetActive(true);
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Fawn"))
        {
            pickUpText.gameObject.SetActive(false);
            pickUpAllowed = false;
        }
    }

    private void PickUp()
    {
        pickUpText.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}