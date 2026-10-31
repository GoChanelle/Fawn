using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSFX;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Play);
    }

    private void Play()
    {
        if (clickSFX == null) return;

        if (SoundManager.instance != null)
            SoundManager.instance.PlaySound(clickSFX);
        else
            AudioSource.PlayClipAtPoint(clickSFX, Camera.main.transform.position);
    }
}