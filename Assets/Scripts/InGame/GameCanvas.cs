using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
