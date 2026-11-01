using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private string menuScene = "MainMenu";

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(GoToMenu);
    }

    private void GoToMenu()
    {
        GameProgress.Reset();
        CollectedPickups.Reset();
        InsideInventoryController.ResetInventory();

        if (LevelManager.Instance != null)
            LevelManager.Instance.LoadScene(menuScene, "CrossFade");  
        else
            SceneManager.LoadScene(menuScene);              
    }
}