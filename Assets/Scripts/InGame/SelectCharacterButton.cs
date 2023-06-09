using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToSelectCharacterButton : MonoBehaviour
{
    Button sceneChangeButton;

    // Start is called before the first frame update
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
