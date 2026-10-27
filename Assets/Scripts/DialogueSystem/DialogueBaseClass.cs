using System.Collections;
using UnityEngine;
using TMPro;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; protected set; }
        public bool typingDone { get; protected set; }
        [SerializeField] protected bool holdForChoice;

        protected void ResetState()
        {
            finished = false;
            typingDone = false;
        }

        protected IEnumerator WriteText(string input, TMP_Text textHolder, float delay, AudioClip sound, float delayBetweenLines)
        {
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];

                if (SoundManager.instance != null && sound != null)
                    SoundManager.instance.PlaySound(sound);

                yield return new WaitForSeconds(delay);
            }

            typingDone = true;

            if (holdForChoice) yield break;

            yield return new WaitUntil(() => Input.GetMouseButton(0));
            finished = true;
        }
    }
}