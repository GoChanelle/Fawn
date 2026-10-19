using System.Collections;
using UnityEngine;

public class CrossFade : SceneTransition
{
    public CanvasGroup crossFade;
    public float duration = 1f;

    public override IEnumerator TransitionOut()
    {
        yield return Fade(0f, 1f);
    }

    public override IEnumerator TransitionIn()
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            crossFade.alpha = Mathf.Lerp(from, to, time / duration);
            yield return null;
        }

        crossFade.alpha = to;
    }
}