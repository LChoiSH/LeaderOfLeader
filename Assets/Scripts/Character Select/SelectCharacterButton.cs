using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCharacterButton : MonoBehaviour
{
    Button sceneChangeButton;

    void Start()
    {
        sceneChangeButton = GetComponent<Button>();
        sceneChangeButton.onClick.AddListener(GoToCharacterSelectScene);
    }

    private void GoToCharacterSelectScene()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
}
