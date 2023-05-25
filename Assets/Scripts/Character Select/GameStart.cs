using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public Button startButton;
    public TextMeshProUGUI loadingProgressText;

    void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(() => LoadScene("GameScene"));

        if(DataManager.instance.currentCharacter == "")
        {
            startButton.interactable = false;
        }
    }

    private void Update()
    {
        if (DataManager.instance.currentCharacter != "")
        {
            startButton.interactable = true;
        }
    }

    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        DataManager.instance.isLoading= true;
        startButton.interactable = false;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            // 0 ~ 0.9
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            loadingProgressText.text = "Now Loading: " + (progress * 100).ToString() + "%";

            yield return null; // wait 1 frame
        }

        startButton.interactable = true;
        DataManager.instance.isLoading = false;
    }
}
