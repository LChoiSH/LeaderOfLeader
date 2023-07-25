using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    // enemy spawn
    HashSet<GameObject> enemies;

    public Vector3 mapBound;
    public float spawnExceptRange = 10.0f;
    public GameObject[] spawnPrefabs;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new HashSet<GameObject>();

        GameObject floor = GameObject.Find("Floor");
        Vector3 mapSize = floor.GetComponent<MeshRenderer>().bounds.size;
        mapBound = mapSize;

        SpawnEnemy(DataManager.instance.gameLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy(int enemyNum)
    {
        float randomX, randomZ;
        int randomIndex;
        Vector3 spawnPos;

        for (int i = 0; i < enemyNum; i++)
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

        if (enemies.Count == 0)
        {
            DataManager.instance.gameLevel++;

            GameController.instance.isClear = true;
            GameController.instance.nextLevelDoor.LightOn();
        }
    }

    public void GetScore(int value)
    {
        DataManager.instance.currentScore += value;
        scoreText.text = DataManager.instance.currentScore.ToString();
    }
}
