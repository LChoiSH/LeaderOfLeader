using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReplayButton : MonoBehaviour
{
    private Button button;
    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        button = GetComponent<Button>();
        button.onClick.AddListener(gameController.GameRestart);
    }
}
