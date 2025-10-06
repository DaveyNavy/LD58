using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutText;
    [SerializeField] private Image blackImage;


    private void Start()
    {
        StartCoroutine(Tutorial());
    }

    private IEnumerator Tutorial()
    {
        // Fade blackImage alpha to 0 over 2 seconds
        float duration = 2f;
        float elapsed = 0f;
        Color color = blackImage.color;
        float startAlpha = color.a;
        float endAlpha = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            blackImage.color = color;
            yield return null;
        }

        // Ensure alpha is exactly 0
        color.a = endAlpha;
        blackImage.color = color;

        // Wait 1 second more
        yield return new WaitForSeconds(1f);

        StartCoroutine(TypeText("Welcome, my special creation"));
        yield return new WaitForSeconds(1f);

        StartCoroutine(TypeText("Move with WASD, please."));
        yield return new WaitForSeconds(1f);

        
    }

    private IEnumerator TypeText(string text, float charDelay = 0.05f)
    {
        tutText.text = text;
        tutText.maxVisibleCharacters = 0;
        for (int i = 1; i <= text.Length; i++)
        {
            tutText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(charDelay);
        }
    }
}
