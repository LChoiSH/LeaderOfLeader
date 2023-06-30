using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWrap : MonoBehaviour
{
    public CharacterInfo[] characterList;
    public int currentCharacter = 0;
    public Button prevButton;
    public Button nextButton;
    public float characterDistance = 5.0f;
    public InformationBox infoBox;
    Vector3 characterLoc = Vector3.zero;

    void Start()
    {
        characterList = DataManager.instance.characterList;

        for (int i = 0; i < characterList.Length; i++)
        {
            GameObject prefabObject = Resources.Load<GameObject>("Prefabs/Character/" + characterList[i].prefab);
            Instantiate(prefabObject, new Vector3(i * characterDistance, 0, 0), Quaternion.Euler(0, 120, 0), transform);
        }

        currentCharacter = 0;

        prevButton.onClick.AddListener(PrevCharacterSelect);
        nextButton.onClick.AddListener(NextChatacterSelect);

        infoBox.OpenBox(DataManager.instance.characterDictionary[currentCharacter].info);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, characterLoc, Time.deltaTime * 10);
    }

    private void CharacterChange(int changeCharacterId)
    {
        DataManager.instance.SetCurrentCharacter(changeCharacterId);

        currentCharacter = changeCharacterId;
        characterLoc.x = -currentCharacter * characterDistance;

        infoBox.OpenBox(DataManager.instance.characterDictionary[currentCharacter].info);
    }

    public void PrevCharacterSelect()
    {
        int nextCharacterId = currentCharacter - 1;
        if (nextCharacterId < 0) nextCharacterId = characterList.Length - 1;

        CharacterChange(nextCharacterId);
    }

    public void NextChatacterSelect()
    {
        int nextCharacterId = (currentCharacter + 1) % characterList.Length;

        CharacterChange(nextCharacterId);
    }
}
