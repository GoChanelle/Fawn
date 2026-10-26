using UnityEngine;

public class Level2Loader : MonoBehaviour
{
    [SerializeField] private GameObject promptObject;
    private bool keyPressedAllowed = false;

    private void Start()
    {
        if (promptObject != null)
            promptObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Fawn"))
        {
            if (promptObject != null)
                promptObject.SetActive(true);
            keyPressedAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Fawn"))
        {
            if (promptObject != null)
                promptObject.SetActive(false);
            keyPressedAllowed = false;
        }
    }

    private void Update()
    {
        if (keyPressedAllowed && Input.GetKeyDown(KeyCode.R))
        {
            LevelManager.Instance.LoadScene("Level2", "CrossFade");
        }
    }
}