using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopupUI : MonoBehaviour
{
    public static ItemPopupUI instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text label;

    private void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    public void Show(Sprite sprite, string itemName)
    {
        icon.sprite = sprite;
        icon.preserveAspect = true;
        label.text = itemName + " Collected!";
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}