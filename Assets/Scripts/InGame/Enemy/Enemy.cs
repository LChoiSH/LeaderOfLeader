using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SocialPlatforms.Impl;

abstract public class Enemy : MonoBehaviour, Damageable, Attackable
{
    protected enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Hit,
        Die
    }

    [SerializeField] EnemyState currentState;

    protected Collider selfCollder;
    protected Rigidbody selfRb;
    protected Animator animator;

    // move
    public float speed;
    float distance = float.MaxValue;
    public float chaseDistance;

    // attack
    public float attackDistance;
    public int damage;
    [SerializeField] public GameObject target;
    protected float attackTime;
    protected float attackDelayTime;
    public float attackSpeed;

    // damaged
    public int maxHp = 100;
    int currentHp;
    HealthBar healthBar;

    // die
    public int score;
    public delegate void DieDelegate(GameObject x);
    DieDelegate dieDelegate;

    virtual protected void Start()
    {
        SetState(EnemyState.Idle);
        target = GameObject.Find("Player");

        attackTime = 100 / attackSpeed;
        attackDelayTime = attackTime;

        animator = GetComponentInChildren<Animator>();

        // Health Bar Setting
        string healthBarPrefabPath = "Prefabs/HealthBar";
        GameObject healthBarPrefab = Resources.Load<GameObject>(healthBarPrefabPath);
        healthBar = Instantiate(healthBarPrefab, transform).GetComponentInChildren<HealthBar>();

        currentHp = maxHp;
        healthBar.SetMaxHealth(maxHp);

        selfRb = gameObject.GetComponent<Rigidbody>();
        selfCollder = gameObject.GetComponent<BoxCollider>();
    }

    // TODO: animation
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Hit:
                break;
            case EnemyState.Die:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            if (other.gameObject.CompareTag("Member"))
            {
                Attack(other.GetComponent<Damageable>());
            }
            else if (other.transform.parent != null && other.transform.parent.CompareTag("Member"))
            {
                Attack(other.GetComponentInParent<Damageable>());
            }
        }
        catch
        {

        }
    }

    protected void SetState(EnemyState state)
    {
        currentState = state;
    }

    virtual protected void Idle()
    {
        distance = (target.transform.position - transform.position).magnitude;

        if (distance < chaseDistance)
        {
            animator.SetTrigger("IdleToMove");
            SetState(EnemyState.Move);
        }
    }
    virtual protected void Move() {
        distance = (target.transform.position - transform.position).magnitude;

        if (distance < attackDistance)
        {
            animator.SetTrigger("MoveToAttackDelay");
            SetState(EnemyState.Attack);
        }
        else if (distance > chaseDistance)
        {
            animator.SetTrigger("IdleToMove");
            SetState(EnemyState.Idle);
        } else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            transform.LookAt(target.transform.position);
        }
    }
    abstract protected void Attack();

    public void OnEndHit()
    {
        SetState(EnemyState.Idle);
    }

    public void Damaged(int damage)
    {
        currentHp -= damage;
        healthBar.SetHealth(currentHp);

        if (currentHp <= 0)
        {
            SetState(EnemyState.Die);
            gameObject.tag = "Untagged";
            animator.SetTrigger("Die");
            Die();
        }
        else
        {
            SetState(EnemyState.Hit);
            animator.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        GameController.instance.GetScore(score);

        SetState(EnemyState.Die);
        selfCollder.enabled = false;
        Destroy(selfRb);

        dieDelegate(gameObject);

        Invoke("DestroySelf", 3.0f);

        StartCoroutine(DestroySelf());
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }

    public void AddDieDelegate(DieDelegate delegateFunction)
    {
        dieDelegate += delegateFunction;
    }

    public void Attack(Damageable damageable)
    {
        damageable.Damaged(damage);
    }

    public int GetDamage()
    {
        return damage;
    }
}
