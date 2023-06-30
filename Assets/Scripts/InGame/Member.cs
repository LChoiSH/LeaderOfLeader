using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Member : MonoBehaviour, Damageable

{
    [SerializeField] bool isLoading = true;

    // move
    public GameObject followingTarget;

    // prefab
    protected GameObject myPrefab;
    protected CharacterInfo myCharacterInfo;
    public GameController gameController;

    // speed
    [SerializeField]protected float speed = 6.0f;

    // attack
    [SerializeField] protected float attackSpeed = 3.0f;
    [SerializeField] Missile missile;
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
        if (!gameController.isGameActive) Die();
        Move();
    }

    public void SetCharacterInfo (CharacterInfo characterInfo)
    {
        myCharacterInfo= characterInfo;

        maxHp = myCharacterInfo.hp;
        currentHp = maxHp;
        healthBar = gameObject.GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(maxHp);

        attackSpeed = characterInfo.attackSpeed;

        string characterPrefabName = DataManager.instance.characterDictionary[myCharacterInfo.id].prefab;
        string characterPrefabPath = "Prefabs/Character/" + characterPrefabName;
        myPrefab = Resources.Load<GameObject>(characterPrefabPath);
        Instantiate(myPrefab, transform);

        string missilePrefabName = DataManager.instance.characterDictionary[myCharacterInfo.id].missilePrefab;
        string missilePrefabPath = "Prefabs/Missile/" + missilePrefabName;

        missile = Resources.Load<GameObject>(missilePrefabPath).GetComponent<Missile>();

        missileObjectPool = new ObjectPool<Missile>(missile, 20);

        animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.SetFloat("Speed_f", speed / 10);

        StartCoroutine(FireClosestTarget());

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

        if (currentHp <= 0) Die();
    }

    protected virtual void Move()
    {
        if (currentHp <= 0) return;
        
        transform.position = Vector3.Lerp(transform.position, followingTarget.transform.position, speed * Time.deltaTime);
        transform.LookAt(followingTarget.transform.position);
    }

    private void Die()
    {
        currentHp = 0;
        animator.SetInteger("DeathType_int", 2);
        animator.SetBool("Death_b", true);

        Invoke("SetActiveFalse", 5f);
        return;
    }

    private void SetActiveFalse()
    {
        transform.gameObject.SetActive(false);
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
            attackTargetArray.Concat(GameObject.FindGameObjectsWithTag("Prison")).ToArray();

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
