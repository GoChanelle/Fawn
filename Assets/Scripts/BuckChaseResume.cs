using UnityEngine;

public class BuckChaseResume : MonoBehaviour
{
    [SerializeField] private BuckChase buck;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float graceSeconds = 3f;   // head start after arriving

    private void Start()
    {
        if (!GameProgress.BuckChasing) return;   // Buck stays inactive

        buck.transform.position = spawnPoint.position;
        buck.gameObject.SetActive(true);

        var rb = buck.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
        
        buck.StartChaseWithDelay(graceSeconds);
    }
}