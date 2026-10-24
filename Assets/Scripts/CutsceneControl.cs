using UnityEngine;

public class CutsceneControl : MonoBehaviour
{
    void OnEnable()
    {
        LevelManager.Instance.LoadScene("Level1", "CrossFade");
    }
}