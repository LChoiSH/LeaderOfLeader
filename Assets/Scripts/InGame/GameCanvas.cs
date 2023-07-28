using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;

    public Leader leader;

    public TextMeshProUGUI skillTimeText;
    public Image skillTimeImage;
    public RawImage skillImage;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //player = GameObject.Find("Player");
    }

    public void SettingLeader(Leader leader)
    {
        this.leader = leader;
    }

    public void SettingSkillImage(Texture2D imageTexture)
    {
        skillImage.texture = imageTexture;
    }

    private void FixedUpdate()
    {
        showSkillTime();
    }

    private void showSkillTime()
    {
        skillTimeImage.fillAmount = leader.currentSkillTime / leader.skillTime;

        if(skillTimeImage.fillAmount > 0)
        {
            skillTimeText.text = Mathf.Ceil(leader.currentSkillTime).ToString();
        } else
        {
            skillTimeText.text = "";
        }
    }

    public void SettinScoreText(int value)
    {
        scoreText.text = value.ToString();
    }

    public void SettinLevelText(int value)
    {
        levelText.text = value.ToString();
    }
}
