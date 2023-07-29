using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public CharacterInfo[] characterList;
    public Dictionary<int, RewardInfo> rewardDictionary = new Dictionary<int, RewardInfo>();
    public RewardInfo[] rewardList;
    public Dictionary<int, CharacterInfo> characterDictionary = new Dictionary<int, CharacterInfo>();
    public bool isLoading = false;
    private int highScore;
    public int currentCharacter { get; set; }

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { return; }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        isLoading = true;

        GetCharacterData();
        GetRewardData();
        highScore = PlayerPrefs.GetInt("highScore");
        isLoading = false;
    }

    void GetCharacterData()
    {
        TextAsset characterJson = Resources.Load<TextAsset>("Data/CharacterData");
        string jsonString = characterJson.ToString();

        CharacterData data = JsonUtility.FromJson<CharacterData>(jsonString);
        characterList = data.characterList;

        foreach (CharacterInfo characterInfo in characterList)
        {
            characterDictionary.Add(characterInfo.id, characterInfo);
        }

        if(characterList.Length > 0)
        {
            currentCharacter = characterList[0].id;
        }
    }

    void GetRewardData()
    {
        TextAsset rewardJson = Resources.Load<TextAsset>("Data/RewardData");
        string jsonString = rewardJson.ToString();

        RewardData data = JsonUtility.FromJson<RewardData>(jsonString);
        rewardList = data.rewardList;

        for(int i = 0;i < rewardList.Length;i++)
        {
            rewardDictionary.Add(i, rewardList[i]);
        }
    }

    public int GetHighScore() { return highScore; }

    public void SetHighScore(int newScore)
    {
        if(highScore < newScore)
        {
            highScore = newScore;
            PlayerPrefs.SetInt("highScore", highScore);
        }
    }
}
