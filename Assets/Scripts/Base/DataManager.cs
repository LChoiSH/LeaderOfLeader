using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public int currentCharacterId;
    public CharacterInfo[] characterList;
    public Dictionary<int, CharacterInfo> characterDictionary = new Dictionary<int, CharacterInfo>();
    public bool isLoading = false;

    private int highScore;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { return; }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        GetCharacterData();
        highScore = PlayerPrefs.GetInt("highScore");

    }

    void GetCharacterData()
    {
        isLoading = true;

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
            currentCharacterId = characterList[0].id;
        }

        isLoading = false;
    }

    public void SetCurrentCharacter(int x)
    {
        currentCharacterId = x;
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
