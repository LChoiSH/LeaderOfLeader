
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Android;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    GameController gameController;
    public GameObject rewardScreen;
    public GameObject rewardButtonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RewardScreenIn()
    {
        gameObject.SetActive(true);

        float positionY = -200;

        for(int i = 0;i < 3;i++)
        {
            Debug.Log("plz 1");

            GameObject buttonGo = Instantiate(rewardButtonPrefab, rewardScreen.transform);
            Debug.Log("plz 2");

            buttonGo.SetActive(true);

            Button button = buttonGo.GetComponent<Button>();
            Debug.Log("plz 3");

            RectTransform rectTransform = button.GetComponent<RectTransform>();
            Debug.Log("plz 4");

            rectTransform.offsetMin = new Vector2(50 , positionY * i);
            rectTransform.offsetMax = new Vector2(50 , positionY * i);
            rectTransform.sizeDelta = new Vector2(0, 200);
            
            Debug.Log("plz 5");

            //TextMeshProUGUI nameText = button.transform.Find("Reward name text").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI nameText = button.GetComponentInChildren<TextMeshProUGUI>();
            //TextMeshProUGUI infoText = button.transform.Find("Reward info text").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI infoText = button.GetComponentInChildren<TextMeshProUGUI>();
            Debug.Log("plz 6");

            RewardInfo rewardInfo = DataManager.instance.rewardList[Random.Range(0, DataManager.instance.rewardList.Length)];
            Debug.Log("plz 7");

            nameText.text = rewardInfo.name;
            infoText.text = rewardInfo.info;
            Debug.Log("plz 8");

        }
        Debug.Log("plz 2");


        StartCoroutine(RewardScreenFadeIn());
    }

    public void RewardScreenOut()
    {
        StartCoroutine(RewardScreenFadeOut());
    }

    IEnumerator RewardScreenFadeIn()
    {
        Debug.Log("rewardScreenFadeIn");

        float fadeDuration = 3.0f;

        CanvasGroup screenCanvasGroup = rewardScreen.GetComponent<CanvasGroup>();
        screenCanvasGroup.alpha = 0;
        rewardScreen.SetActive(true);

        float currentTime = 0;

        while (currentTime < fadeDuration || screenCanvasGroup.alpha < 1)
        {
            screenCanvasGroup.alpha = currentTime / fadeDuration;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator RewardScreenFadeOut()
    {
        float fadeDuration = 3.0f;

        CanvasGroup screenCanvasGroup = rewardScreen.GetComponent<CanvasGroup>();
        screenCanvasGroup.alpha = 0;

        float currentTime = 0;

        while (currentTime < fadeDuration || screenCanvasGroup.alpha > 0)
        {
            screenCanvasGroup.alpha = (fadeDuration - currentTime) / fadeDuration;
            currentTime += Time.deltaTime;
            yield return null;
        }

        rewardScreen.SetActive(false);
    }
}
