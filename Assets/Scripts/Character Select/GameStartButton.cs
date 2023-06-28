using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviour
{
    public Button startButton;
    public TextMeshProUGUI loadingProgressText;

    // loading
    public GameObject LoadingScreen;

    void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(() => LoadScene("GameScene"));

        loadingProgressText.text = "";

        LoadingScreen.SetActive(false);
    }

    public void SetInteractable(bool interactable)
    {
        startButton.interactable = interactable;
    }

    private void LoadScene(string sceneName)
    {
        StartCoroutine(FadeIn());
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        DataManager.instance.isLoading= true;
        startButton.interactable = false;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            loadingProgressText.text = "Now Loading... " + (progress * 100).ToString() + "%";

            yield return null;
        }

        startButton.interactable = true;
        DataManager.instance.isLoading = false;
    }

    IEnumerator FadeIn()
    {
        float fadeDuration = 1.0f;

        CanvasGroup screenCanvasGroup = LoadingScreen.GetComponent<CanvasGroup>();
        screenCanvasGroup.alpha = 0;
        LoadingScreen.SetActive(true);

        float currentTime = 0;

        while (currentTime < fadeDuration || screenCanvasGroup.alpha < 1)
        {
            screenCanvasGroup.alpha = currentTime / fadeDuration;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
