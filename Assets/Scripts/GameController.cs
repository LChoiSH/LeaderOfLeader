using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Game Over Screen
    public GameObject gameOverScreen;

    public bool isGameActive = true;

    // Game Level
    private int gameLevel = 1;
    public TextMeshProUGUI levelText;

    // enemy spawn Time
    [SerializeField] float spawnTime = 2.0f;

    private float mapBound = 40.0f;
    public float spawnExceptRange = 10.0f;
    private GameObject player;
    public GameObject[] spawnPrefabs;
    public TextMeshProUGUI scoreText;
    private int score = 0;

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

        GameStart();
    }

    public void GameStart()
    {
        isGameActive = true;
        gameOverScreen.SetActive(false);
        GetScore(0);
        StartCoroutine(SpawnEnemy());

    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverScreen.SetActive(true);
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
                randomX = Random.Range(-mapBound, mapBound);
                randomZ = Random.Range(-mapBound, mapBound);
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

    public void GetScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();

        if(score % 10 == 0)
        {
            LevelUp(score / 10);
        }
    }

    public void LevelUp(int level)
    {
        gameLevel= level+1;
        levelText.text = gameLevel.ToString();
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        // spawn Prison
        Vector3 spawnPos = new Vector3(Random.Range(-mapBound, mapBound), 2.5f, Random.Range(-mapBound, mapBound));
        Instantiate(prison, spawnPos, randomRotation);
    }

}
