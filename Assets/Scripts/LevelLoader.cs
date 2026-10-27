using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [Header("Where to go")]
    [SerializeField] private string nextSceneName;
    [SerializeField] private string transitionName = "CrossFade";

    [Header("Prompt")]
    [SerializeField] private GameObject promptObject;   // the small "R" popup

    [Header("Optional: stop Buck before leaving (good ending)")]
    [SerializeField] private BuckChase buck;

    private bool playerInRange;
    private bool loading;

    private void Start()
    {
        if (promptObject != null) promptObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.name.Equals("Fawn")) return;
        playerInRange = true;
        if (promptObject != null) promptObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.name.Equals("Fawn")) return;
        playerInRange = false;
        if (promptObject != null) promptObject.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange || loading || !Input.GetKeyDown(KeyCode.R)) return;

        loading = true; // stops a double press from loading twice
        if (buck != null) buck.StopChase();
        LevelManager.Instance.LoadScene(nextSceneName, transitionName);
    }
}