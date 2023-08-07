using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCharacterButton : MonoBehaviour
{
    Button sceneChangeButton;
    public LoadingCanvas loadingCanvas;

    void Start()
    {
        sceneChangeButton = GetComponent<Button>();
        sceneChangeButton.onClick.AddListener(GoToCharacterSelectScene);
    }

    private void GoToCharacterSelectScene()
    {
        StartCoroutine(loadingCanvas.FadeIn());

        GameController.instance.GameSettingReset();

        StartCoroutine(LoadSceneAsync("CharacterSelect"));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        float loadingTime = 0;

        while (!asyncOperation.isDone)
        {
            loadingTime += Time.deltaTime;

            if (loadingTime > 1) asyncOperation.allowSceneActivation = true;

            yield return null;
        }
    }
}
