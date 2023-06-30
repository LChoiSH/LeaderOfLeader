using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
    Attack,
    Hit,
    Die
}

public class Enemy : MonoBehaviour, Attackable, Damageable
{
    // Enemy FSM
    [SerializeField] private EnemyState currentState;

    private GameObject target;
    public float speed = 1.0f;
    public int damage = 1;
    [SerializeField] int score = 1;
    GameController gameController;
    Rigidbody selfRb;
    BoxCollider selfCollder;

    // chase
    private float distance;
    [SerializeField] private float chaseDistance = 40.0f;

    // attack
    [SerializeField] private float attackDistance = 5.0f;

    // animation
    protected Animator animator;

    // damaged
    [SerializeField] int maxHp = 100;
    [SerializeField] private int currentHp;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        currentState = EnemyState.Idle;
        target = GameObject.Find("Player");

        animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.SetFloat("moveSpeed", 0);

        healthBar = gameObject.GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(maxHp);
        currentHp = maxHp;

        selfRb = gameObject.GetComponent<Rigidbody>();
        selfCollder = gameObject.GetComponent<BoxCollider>();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleStateUpdate();
                break;
            case EnemyState.Move:
                MoveStateUpdate();
                break;
            case EnemyState.Attack:
                AttackStateUpdate();
                break;
            case EnemyState.Hit:
                HitStateUpdate();
                break;
            case EnemyState.Die:
                break;
        }
    }

    public void SetState(EnemyState state)
    {
        currentState = state;
    }

    private void IdleStateUpdate()
    {
        distance = (target.transform.position - transform.position).magnitude;

        if (distance < attackDistance)
        {
            animator.SetTrigger("triggerAttack");
            SetState(EnemyState.Attack);
        } else if (distance < chaseDistance)
        {
            animator.SetFloat("moveSpeed", 1);
            SetState(EnemyState.Move);
        }
    }

    private void MoveStateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.LookAt(target.transform.position);

        float distance = (target.transform.position - transform.position).magnitude;

        if (distance < attackDistance)
        {
            animator.SetTrigger("triggerAttack");
            animator.SetFloat("moveSpeed", 0);
            SetState(EnemyState.Attack);
        } else if (distance > chaseDistance)
        {
            animator.SetFloat("moveSpeed", 0);
            SetState(EnemyState.Idle);
        }
    }

    private void AttackStateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        transform.LookAt(target.transform.position);
    }

    private void HitStateUpdate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Member") || other.CompareTag("Player")) {
            Attack(other.GetComponent<Damageable>());
        }
    }

    public void Attack(Damageable damageable)
    {
        damageable.Damaged(damage);
    }

    public void Damaged(int damage)
    {
        currentHp -= damage;
        healthBar.SetHealth(currentHp);

        if (currentHp <= 0)
        {
            SetState(EnemyState.Die);
            animator.Play("Die", 0, 0f);
            Die();
        } else
        {
            SetState(EnemyState.Hit);
            animator.Play("GetHit", 0, 0f);
            animator.SetBool("isHit", true);
        }
    }

    public void OnEndAttack()
    {
        SetState(EnemyState.Idle);
    }

    public void OnEndHit()
    {
        animator.SetBool("isHit", false);
        SetState(EnemyState.Idle);
    }

    private void Die()
    {
        gameController.GetScore(score);

        SetState(EnemyState.Die);
        selfCollder.enabled = false;
        Destroy(selfRb);

        Invoke("DestroySelf", 3.0f);

        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }

}
