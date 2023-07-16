using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Game active
    public bool isGameActive = true;
    public bool isClear = false;
    public bool isReward = false;

    // Game Over Screen
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI highScoreTitleText;
    public TextMeshProUGUI highScoreText;

    // Reward Manager
    public RewardManager rewardManager;

    // Game Level
    private int gameLevel = 1;
    public TextMeshProUGUI levelText;
    public NextLevelDoor nextLevelDoor;

    // enemy spawn
    [SerializeField] float spawnTime = 2.0f;
    HashSet<GameObject> enemies;

    public Vector3 mapBound;
    public float spawnExceptRange = 10.0f;
    private GameObject player;
    public GameObject[] spawnPrefabs;
    public TextMeshProUGUI scoreText;

    // prison prefab
    [SerializeField] GameObject prison;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        CharacterInfo leaderCharacter = DataManager.instance.characterDictionary[DataManager.instance.currentCharacter];
        string characterName = leaderCharacter.name.Replace(" ", "");
        System.Type componentType = System.Type.GetType(characterName);

        if (componentType != null)
        {
            Component newComponent = player.AddComponent(componentType);
        }

        enemies = new HashSet<GameObject>();

        isClear = false;

        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        isGameActive = false;
        gameOverScreen.SetActive(false);
        //GetScore(0);
        //StartCoroutine(SpawnEnemy());

        // ????
        GameObject floor = GameObject.Find("Floor");

        Vector3 mapSize = floor.GetComponent<MeshRenderer>().bounds.size;
        mapBound = mapSize;

        SpawnEnemy(DataManager.instance.gameLevel);

        yield return new WaitForSeconds(1.5f);

        isGameActive = true;
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        isGameActive = false;
        int score = DataManager.instance.currentScore;
        gameOverScoreText.text = score.ToString();
        
        int highScore = DataManager.instance.GetHighScore();
        if (score > highScore)
        {
            DataManager.instance.SetHighScore(score);
            highScoreTitleText.text = "New High Score!!!!!";
        }

        highScoreText.text = DataManager.instance.GetHighScore().ToString();

        StartCoroutine(GameOverScreenFadeIn());
    }

    IEnumerator SpawnEnemy()
    {
        float randomX, randomZ;
        int randomIndex;
        Vector3 spawnPos;
        float playerDistance;

        while(isGameActive)
        {
            yield return new WaitForSeconds(spawnTime / gameLevel);

            do
            {
                randomX = Random.Range(-mapBound.x / 2, mapBound.x / 2);
                randomZ = Random.Range(-mapBound.z / 2, mapBound.z / 2);
                randomIndex = Random.Range(0, spawnPrefabs.Length);

                spawnPos = new Vector3(randomX, 0, randomZ);

                playerDistance = (player.transform.position - spawnPos).sqrMagnitude;

                if (playerDistance >= spawnExceptRange)
                {
                    Instantiate(spawnPrefabs[randomIndex], spawnPos, spawnPrefabs[randomIndex].transform.rotation);
                }

            } while (playerDistance < spawnExceptRange);
        }
    }

    public void SpawnEnemy(int enemyNum)
    {
        float randomX, randomZ;
        int randomIndex;
        Vector3 spawnPos;

        for(int i = 0;i < enemyNum;i++)
        {
            randomX = Random.Range(-mapBound.x / 2, mapBound.x / 2);
            randomZ = Random.Range(-mapBound.z / 2, mapBound.z / 2);
            randomIndex = Random.Range(0, spawnPrefabs.Length);

            spawnPos = new Vector3(randomX, 0, randomZ);
            GameObject spawnEnemy = Instantiate(spawnPrefabs[randomIndex], spawnPos, spawnPrefabs[randomIndex].transform.rotation);
            Enemy inputEnemy = spawnEnemy.GetComponent<Enemy>();
            inputEnemy.AddDieDelegate(RemoveEnemy);

            enemies.Add(spawnEnemy);
        }
    }

    void RemoveEnemy(GameObject removeEnemy)
    {
        enemies.Remove(removeEnemy);

        if(enemies.Count == 0)
        {
            DataManager.instance.gameLevel++;

            isClear = true;
            nextLevelDoor.LightOn();
        }
    }

    public void GetScore(int value)
    {
        DataManager.instance.currentScore += value;
        scoreText.text = DataManager.instance.currentScore.ToString();
    }

    //public void GetScore(int value)
    //{
    //    score += value;
    //    scoreText.text = score.ToString();

    //    if(score % 10 == 0)
    //    {
    //        LevelUp(score / 10);
    //    }
    //}

    public void LevelUp(int level)
    {
        gameLevel= level+1;
        levelText.text = gameLevel.ToString();
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        // spawn Prison
        //Vector3 spawnPos = new Vector3(Random.Range(-mapBound, mapBound), 2.5f, Random.Range(-mapBound, mapBound));
        //Instantiate(prison, spawnPos, randomRotation);
    }

    IEnumerator GameOverScreenFadeIn()
    {
        float fadeDuration = 3.0f;

        CanvasGroup screenCanvasGroup = gameOverScreen.GetComponent<CanvasGroup>();
        screenCanvasGroup.alpha = 0;
        gameOverScreen.SetActive(true);
        
        float currentTime = 0;

        while (currentTime < fadeDuration || screenCanvasGroup.alpha < 1)
        {
            screenCanvasGroup.alpha = currentTime / fadeDuration;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
