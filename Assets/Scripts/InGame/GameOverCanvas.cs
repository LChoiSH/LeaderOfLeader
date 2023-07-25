using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameOverCanvas : MonoBehaviour
{
    public static GameOverCanvas instance;


    public CanvasGroup gameOverScreen;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI highScoreTitleText;
    public TextMeshProUGUI highScoreText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }

    public void GameOverScreenOn()
    {
        int score = DataManager.instance.currentScore;
        gameOverScoreText.text = score.ToString();

        int highScore = DataManager.instance.GetHighScore();
        if (score > highScore)
        {
            DataManager.instance.SetHighScore(score);
            highScoreTitleText.text = "New High Score!!!!!";
        }

        highScoreText.text = DataManager.instance.GetHighScore().ToString();

        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float fadeDuration = 3.0f;

        gameOverScreen.alpha = 0;
        gameObject.SetActive(true);

        float currentTime = 0;

        while (currentTime < fadeDuration || gameOverScreen.alpha < 1)
        {
            gameOverScreen.alpha = currentTime / fadeDuration;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
