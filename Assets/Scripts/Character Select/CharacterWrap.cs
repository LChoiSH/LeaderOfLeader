using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWrap : MonoBehaviour
{
    public CharacterInfo[] characterList;
    public int currentCharacter = 0;
    public Button prevButton;
    public Button nextButton;
    public float characterDistance = 5.0f;
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

        prevButton.onClick.AddListener(prevCharacterSelect);
        nextButton.onClick.AddListener(nextChatacterSelect);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, characterLoc, Time.deltaTime * 10);
    }

    public void prevCharacterSelect()
    {
        currentCharacter--;
        if (currentCharacter < 0) currentCharacter = characterList.Length - 1;
        DataManager.instance.SetCurrentCharacter(currentCharacter);
        characterLoc.x = -currentCharacter * 5;
    }

    public void nextChatacterSelect()
    {
        currentCharacter = (currentCharacter + 1) % characterList.Length;
        DataManager.instance.SetCurrentCharacter(currentCharacter);
        characterLoc.x = -currentCharacter * 5;
    }

}
