using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : MonoBehaviour
{
    public CanvasGroup loadingScreen;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        loadingScreen.alpha = 1.0f;

        float fadeDuration = 1.0f;
        float currentTime = fadeDuration;

        while (loadingScreen.alpha > 0)
        {
            loadingScreen.alpha = Mathf.Clamp(currentTime / fadeDuration, 0, 1);
            currentTime -= Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    public IEnumerator FadeIn()
    {
        float fadeDuration = 1.0f;

        loadingScreen.alpha = 0;
        gameObject.SetActive(true);

        float currentTime = 0;

        while (currentTime < fadeDuration || loadingScreen.alpha < 1)
        {
            loadingScreen.alpha = Mathf.Clamp(currentTime / fadeDuration, 0, 1);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
