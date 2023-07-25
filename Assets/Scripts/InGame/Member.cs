using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Member : MonoBehaviour, Damageable

{
    bool isLoading = true;

    // move
    public GameObject followingTarget;

    // prefab
    protected CharacterInfo myCharacterInfo;
    public GameController gameController;

    // speed
    [SerializeField]protected float speed = 6.0f;

    // attack
    [SerializeField] protected float attackSpeed = 3.0f;
    private ObjectPool<Missile> missileObjectPool;
    private GameObject[] attackTargetArray;
    private float targetDistance, closestTargetDistance;
    private float attackRange = 50.0f; 

    // damaged
    protected int maxHp = 100;
    protected int currentHp = 100;
    private bool isDamaged = false;
    private float hitTime = 1.5f;
    protected HealthBar healthBar;

    // animation
    protected Animator animator;

    private void Awake()
    {
        isLoading = true;
    }

    protected virtual void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    private void FixedUpdate()
    {
        if (isLoading) return;
        Move();
    }

    #nullable enable
    public void SetCharacterInfo (CharacterInfo characterInfo, GameObject? followingTarget)
    {
        Debug.Log(0);

        myCharacterInfo= characterInfo;

        attackSpeed = characterInfo.attackSpeed;

        // Character Prefab Setting
        string characterPrefabPath = "Prefabs/Character/" + DataManager.instance.characterDictionary[myCharacterInfo.id].prefab;
        GameObject characterPrefab = Resources.Load<GameObject>(characterPrefabPath);
        Instantiate(characterPrefab, transform);

        // Health Bar Setting
        string healthBarPrefabPath = "Prefabs/HealthBar";
        GameObject healthBarPrefab = Resources.Load<GameObject>(healthBarPrefabPath);
        healthBar = Instantiate(healthBarPrefab, transform).GetComponentInChildren<HealthBar>();

        maxHp = myCharacterInfo.hp;
        currentHp = maxHp;
        healthBar.SetMaxHealth(maxHp);

        // Missile Setting
        string missilePrefabPath = "Prefabs/Missile/" + DataManager.instance.characterDictionary[myCharacterInfo.id].missilePrefab;
        Missile missile = Resources.Load<GameObject>(missilePrefabPath).GetComponent<Missile>();

        missileObjectPool = new ObjectPool<Missile>(missile, 20);

        // Animator Setting
        animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.SetTrigger("TriggerRun");

        StartCoroutine(FireClosestTarget());

        if(followingTarget != null)
        {
            this.followingTarget = followingTarget;
            transform.position = this.followingTarget.transform.position;
        }

        isLoading = false;
    }

    public virtual void Damaged(int damage)
    {
        if (!isDamaged)
        {
            currentHp -= damage;
            healthBar.SetHealth(currentHp);
            isDamaged = true;
            StartCoroutine(SetInvincible(hitTime));
        }

        if (currentHp <= 0)
        {
            currentHp = 0;
            animator.SetTrigger("TriggerDeath");
            transform.parent = null;
            Invoke("Die", 5.0f);
        }
    }

    private void OnDestroy()
    {
        missileObjectPool.DestroyPool();
    }

    protected virtual void Move()
    {
        if (followingTarget == null) return;
        if (currentHp <= 0) return;
        
        StartCoroutine(FollowTarget());
    }

    IEnumerator FollowTarget()
    {
        Vector3 targetPosition = followingTarget.transform.position;
        Quaternion targetRotation = followingTarget.transform.rotation;
        yield return new WaitForSeconds(0.5f);

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator SetInvincible(float waitTime)
    {

        Renderer[] childrenRenderer = transform.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in childrenRenderer)
        {
            foreach (Material material in rend.materials)
            {
                material.SetColor("_Color", Color.red);
            }
        }

        yield return new WaitForSeconds(waitTime);

        foreach (Renderer rend in childrenRenderer)
        {
            foreach (Material material in rend.materials)
            {
                material.SetColor("_Color", Color.white);
            }
        }

        isDamaged = false;
    }

    IEnumerator FireClosestTarget()
    {
        while (currentHp > 0)
        {
            yield return new WaitForSeconds(attackSpeed);

            attackTargetArray = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject closestTarget = null;
            closestTargetDistance = Mathf.Infinity;

            foreach (GameObject target in attackTargetArray)
            {
                targetDistance = (target.transform.position - transform.position).magnitude;

                if (targetDistance < closestTargetDistance)
                {
                    closestTarget = target;
                    closestTargetDistance = targetDistance;
                }
            }

            if (closestTarget != null && closestTargetDistance <= attackRange)
            {
                Vector3 targetDirection = closestTarget.transform.position - transform.position;

                missileObjectPool.GetNextObject(transform.position, Quaternion.LookRotation(targetDirection));
            }
        }
    }

}
