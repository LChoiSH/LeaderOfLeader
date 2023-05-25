using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public string currentCharacter;
    public Dictionary<string, CharacterInfo> characterDictionary = new Dictionary<string, CharacterInfo>();

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

    // Start is called before the first frame update
    void Start()
    {
        getCharacterData();
    }

    void getCharacterData()
    {
        TextAsset characterJson = Resources.Load<TextAsset>("Data/CharacterData");
        string jsonString = characterJson.ToString();

        // JSON µ•¿Ã≈Õ Deserialize
        CharacterData data = JsonUtility.FromJson<CharacterData>(jsonString);
        Dictionary<string, string> sampleDic = new Dictionary<string, string>();

        foreach (CharacterInfo characterInfo in data.characterList)
        {
            characterDictionary.Add(characterInfo.name, characterInfo);
        }
    }
}
