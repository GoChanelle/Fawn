using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private TMP_Text textHolder;
        [SerializeField] private string input;
        [SerializeField] private float delay;
        [SerializeField] private float delayBetweenLines;
        [SerializeField] private AudioClip sound;

        [SerializeField] private Sprite characterSprite;
        [SerializeField] private Image imageHolder;


        private void Awake()
        {
            textHolder = GetComponent<TMP_Text>();
            textHolder.text = "";

            imageHolder.sprite = characterSprite;
            imageHolder.preserveAspect = true;
        }

        private void Start()
        {
            StartCoroutine(WriteText(input, textHolder, delay, sound, delayBetweenLines));
        }
    }
}