using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _thanksForPlaying;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(float duration = 0.5f)
    {
        StartCoroutine(FadeCanvasGroup(_canvasGroup, _canvasGroup.alpha, 1f, duration));

        if (PlayerStats.Instance.IsGameOver)
        {
            _continueButton.SetActive(false);
            _thanksForPlaying.SetActive(true);
        }
    }

    public void Hide(float duration = 0.5f)
    {
        StartCoroutine(FadeCanvasGroup(_canvasGroup, _canvasGroup.alpha, 0f, duration));
        PlayerStats.Instance.player.OnRespawn();
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = end;
        canvasGroup.interactable = end > 0.99f;
        canvasGroup.blocksRaycasts = end > 0.99f;
    }
}
