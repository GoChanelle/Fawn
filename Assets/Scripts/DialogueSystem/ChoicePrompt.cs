using UnityEngine;
using UnityEngine.UI;

public class ChoicePrompt : MonoBehaviour
{
    [SerializeField] private GameObject choicePanel; 
    [SerializeField] private Button badEndButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private string badEndScene = "BadEnd";

    public bool finished { get; private set; }

    private void Awake()
    {
        badEndButton.onClick.AddListener(OnBadEnd);
        continueButton.onClick.AddListener(OnContinue);
        choicePanel.SetActive(false);
    }

    public void Show()
    {
        finished = false;
        choicePanel.SetActive(true);
    }

    private void OnBadEnd()
    {
        choicePanel.SetActive(false);
        LevelManager.Instance.LoadScene(badEndScene, "CrossFade");
    }

    private void OnContinue()
    {
        choicePanel.SetActive(false);
        finished = true;
    }
}