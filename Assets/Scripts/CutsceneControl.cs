using UnityEngine;

public class CutsceneControl : MonoBehaviour
{
    void OnEnable()
    {
        LevelManager.Instance.LoadScene("SampleScene", "CrossFade");
    }
}