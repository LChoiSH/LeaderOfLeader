using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    // Game active
    public bool isGameActive = true;
    public bool isClear = false;
    public bool isReward = false;

    // Game Over Screen
    public GameOverCanvas gameOverCanvas;

    // Game Level
    public int gameLevel = 1;
    public NextLevelDoor nextLevelDoor;

    // player
    private GameObject player;
    [SerializeField] private GameObject startPoint;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        GetScore(0);
        MakePlayer();
        StartCoroutine(GameStart());
    }
        
    void MakePlayer()
    {
        player = GameObject.Find("Player");

        Member member = player.AddComponent<Member>();

        member.SetCharacterInfo(DataManager.instance.characterList[DataManager.instance.currentCharacter], null);

        CharacterInfo leaderCharacter = DataManager.instance.characterDictionary[DataManager.instance.currentCharacter];
        string characterName = leaderCharacter.name.Replace(" ", "");

        System.Type componentType = System.Type.GetType(characterName);

        if (componentType != null)
        {
            Component newComponent = player.AddComponent(componentType);
        }
    }

    IEnumerator GameStart()
    {
        startPoint = GameObject.Find("StartPoint");
        player.transform.position = startPoint.transform.position;
        player.transform.rotation = startPoint.transform.rotation;

        nextLevelDoor = GameObject.Find("Next Level").GetComponent<NextLevelDoor>();

        isClear = false;
        isGameActive = false;

        GameObject floor = GameObject.Find("Floor");
        Vector3 mapSize = floor.GetComponent<MeshRenderer>().bounds.size;

        yield return new WaitForSeconds(1.5f);

        isGameActive = true;
    }

    public void NextStage()
    {
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().name));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        LevelUp();
        StartCoroutine(GameStart());
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        player.transform.position = startPoint.transform.position;
        StartCoroutine(GameStart());
    }

    public void GameOver()
    {
        isGameActive = false;

        gameOverCanvas.GameOverScreenOn();
    }

    public void GetScore(int value)
    {
        DataManager.instance.currentScore += value;
    }

    public void LevelUp()
    {
        gameLevel++;
    }

}
