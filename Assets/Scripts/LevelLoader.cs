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

    [Header("Optional: gating")]
    [SerializeField] private bool requireLockedRoomCutscene;
    [SerializeField] private string requiredPickupId;   // leave empty for no pickup requirement

    private bool playerInRange;
    private bool loading;

    private bool CanLoad()
    {
        if (requireLockedRoomCutscene && !GameProgress.LockedRoomCutsceneSeen) return false;
        if (!string.IsNullOrEmpty(requiredPickupId) && !CollectedPickups.IsCollected(requiredPickupId)) return false;
        return true;
    }

    private void Start()
    {
        if (promptObject != null) promptObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.name.Equals("Fawn")) return;
        playerInRange = true;
        if (promptObject != null) promptObject.SetActive(CanLoad());
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
        if (!CanLoad()) return;

        loading = true; // stops a double press from loading twice
        if (buck != null) buck.StopChase();
        LevelManager.Instance.LoadScene(nextSceneName, transitionName);
    }
}