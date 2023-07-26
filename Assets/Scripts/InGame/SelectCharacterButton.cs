using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCharacterButton : MonoBehaviour
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
        GameController.instance.GameSettingReset();
        SceneManager.LoadScene("CharacterSelect");
    }
}
