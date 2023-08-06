
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using Unity.VisualScripting;

public class RewardManager : MonoBehaviour
{
    public GameObject rewardScreen;
    public GameObject rewardButtonPrefab;
    public Rewards rewards;
    public LoadingCanvas loadingCanvas;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        rewards = new Rewards();
    }

    public void RewardScreenIn()
    {
        gameObject.SetActive(true);

        float positionY = -200;

        HashSet<int> madeRewards = new HashSet<int>();

        for(int i = 0;i < 3;i++)
        {
            int randomRewardNum = UnityEngine.Random.Range(i, DataManager.instance.rewardList.Length);
            if (!madeRewards.Add(randomRewardNum))
            {
                i--;
                continue;
            }

            RewardInfo rewardInfo = DataManager.instance.rewardList[randomRewardNum];

            GameObject buttonGo = Instantiate(rewardButtonPrefab, rewardScreen.transform);

            buttonGo.SetActive(true);

            Button button = buttonGo.GetComponent<Button>();

            RectTransform rectTransform = button.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector2(0, 0.5f);
            rectTransform.anchoredPosition = new Vector2(1, 0.5f);
            rectTransform.offsetMin = new Vector2(50, positionY * i);
            rectTransform.offsetMax = new Vector2(-50, positionY * i);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 200f);

            TextMeshProUGUI[] texts = button.GetComponentsInChildren<TextMeshProUGUI>();

            TextMeshProUGUI nameText = texts[0];
            TextMeshProUGUI infoText = texts[1];

            button.onClick.AddListener(() => ScreenHide());
            button.onClick.AddListener(() => ReflectRewards(rewardInfo.name));
            button.onClick.AddListener(() => GameController.instance.NextStage());
            button.onClick.AddListener(() => { button.interactable = false; });

            nameText.text = rewardInfo.title;
            infoText.text = rewardInfo.info;
        }

        ScreenShow();
    }

    void ReflectRewards(string methodName)
    {
        // 메서드를 가져올 타입
        System.Type targetType = typeof(Rewards);

        // 메서드의 이름
        string method = methodName;

        // 메서드 정보 가져오기
        MethodInfo methodInfo = targetType.GetMethod(methodName);

        // 가져온 메서드 실행
        if (methodInfo != null)
        {
            object instance = Activator.CreateInstance(targetType);
            methodInfo.Invoke(instance, null);
        }
    }

    void ScreenShow()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    void ScreenHide()
    {
        StartCoroutine(loadingCanvas.FadeIn());
        Time.timeScale = 1;
    }
}
