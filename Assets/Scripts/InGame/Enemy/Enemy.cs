using System.Collections;
using UnityEngine;

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

    [SerializeField] protected EnemyState currentState;

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
    public GameObject target;
    [SerializeField] protected float attackTime;
    [SerializeField] protected float attackDelayTime;
    public float attackSpeed;

    // damaged
    public int maxHp = 100;
    protected int currentHp;
    protected HealthBar healthBar;

    // die
    public int score;
    public delegate void DieDelegate(GameObject x);
    DieDelegate dieDelegate;

    virtual protected void Start()
    {
        SetState(EnemyState.Idle);

        target = (target != null ? target : MovePlayer.instance.gameObject);

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
        if (currentState == EnemyState.Die) return;

        currentState = state;
    }

    virtual protected void Idle()
    {
        if (!GameController.instance.isGameActive) return;

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

    virtual public void Damaged(int damage)
    {
        currentHp -= damage;
        healthBar.SetHealth(currentHp);

        if (currentHp <= 0)
        {
            gameObject.tag = "Untagged";
            animator.SetTrigger("Die");
            SetState(EnemyState.Die);
            Die();
        }
        else
        {
            SetState(EnemyState.Hit);
            animator.SetTrigger("Hit");
        }
    }

    virtual protected void Die()
    {
        GameController.instance.GetScore(score);

        SetState(EnemyState.Die);
        selfCollder.enabled = false;
        Destroy(selfRb);

        dieDelegate(gameObject);

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
