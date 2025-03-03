using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Image imageUI;
    public Text StartText;
    public float fadeDuration = 2.0f;

    void Start()
    {
        SetAlpha(imageUI, 0f);
        SetAlpha(StartText, 0f);

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(5.0f);

        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1f, timeElapsed / fadeDuration);

            SetAlpha(imageUI, alpha);
            SetAlpha(StartText, alpha);

            yield return null;
        }

        SetAlpha(imageUI, 1f);
        SetAlpha(StartText, 1f);
    }

    private void SetAlpha(Graphic graphic, float alpha)
    {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }
}