using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject target;
    public float speed = 1.0f;
    public int damage = 1;
    [SerializeField] int score = 1;
    GameController gameController;

    // damaged
    [SerializeField] int MaxHp = 100;
    private int currentHp;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        target = GameObject.Find("Player");

        currentHp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {

        // 오브젝트를 부드럽게 대상 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.LookAt(target.transform.position);
    }

    private void FixedUpdate()
    {
    }

    public void Damaged(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            gameController.GetScore(score);
            Destroy(gameObject);
        }
    }

}
