using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public float spawnDistance = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new HashSet<GameObject>();

        GameObject floor = GameObject.Find("Floor");
        Vector3 mapSize = floor.GetComponent<MeshRenderer>().bounds.size;
        mapBound = mapSize - new Vector3(1, 1, 1);

        SpawnEnemy(GameController.instance.gameLevel * 3);
    }

    public void SpawnEnemy(int enemyNum)
    {
        float randomX, randomZ;
        int randomIndex;
        Vector3 spawnPos;
        Vector3 startPoint = GameObject.Find("StartPoint").transform.position;

        for (int i = 0; i < enemyNum; i++)
        {
            randomX = Random.Range(-mapBound.x / 2, mapBound.x / 2);
            randomZ = Random.Range(-mapBound.z / 2, mapBound.z / 2);
            randomIndex = Random.Range(0, spawnPrefabs.Length);

            spawnPos = new Vector3(randomX, 0, randomZ);

            if((startPoint - spawnPos).magnitude < spawnDistance)
            {
                i--;
                continue;
            } 

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
            GameController.instance.StageClear();
        }
    }
}
