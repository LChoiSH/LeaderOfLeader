using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void GameOverScreenOn()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float fadeDuration = 3.0f;

        CanvasGroup screenCanvasGroup = gameObject.GetComponent<CanvasGroup>();
        screenCanvasGroup.alpha = 0;
        gameObject.SetActive(true);

        float currentTime = 0;

        while (currentTime < fadeDuration || screenCanvasGroup.alpha < 1)
        {
            screenCanvasGroup.alpha = currentTime / fadeDuration;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
