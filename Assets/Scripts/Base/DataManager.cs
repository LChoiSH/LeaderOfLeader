using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public int currentCharacterId;
    public CharacterInfo[] characterList;
    public Dictionary<int, CharacterInfo> characterDictionary = new Dictionary<int, CharacterInfo>();
    public bool isLoading = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        GetCharacterData();
    }

    void GetCharacterData()
    {
        isLoading = true;

        TextAsset characterJson = Resources.Load<TextAsset>("Data/CharacterData");
        string jsonString = characterJson.ToString();

        // JSON Data Deserialize
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
}
