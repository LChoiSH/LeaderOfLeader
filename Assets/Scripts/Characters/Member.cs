using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Member : MonoBehaviour 

{
    [SerializeField]  bool isLoading = true;

    // move
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    public Vector3 myPath;

    // prefab
    protected GameObject myPrefab;
    protected CharacterInfo myCharacterInfo;

    public GameController gameController;
    public GameObject followingTarget;

    // speed
    protected float speed = 8.0f;

    // attack
    [SerializeField] protected float attackSpeed = 3.0f;
    [SerializeField] Missile missile;
    private ObjectPool<Missile> objectPool;

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

    private void Update()
    {
        if (isLoading) return;
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

        objectPool = new ObjectPool<Missile>(missile, 20);

        animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.SetFloat("Speed_f", speed / 10);

        StartCoroutine(FireClosestTarget());

        isLoading = false;
    }

    public void Damaged(int damage)
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

        if (!gameController.isGameActive) Die();

        Transform tempFollowing = followingTarget.transform.Find("Following Point");
        
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

            GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] prisonList = GameObject.FindGameObjectsWithTag("Prison");
            GameObject[] targets = enemyList.Concat(prisonList).ToArray();

            GameObject closest = null;

            float distance = Mathf.Infinity;

            foreach (GameObject target in targets)
            {
                Vector3 diff = target.transform.position - transform.position;
                float curDistance = diff.sqrMagnitude;

                if (curDistance < distance)
                {
                    closest = target;
                    distance = curDistance;
                }
            }

            if (closest != null)
            {
                Vector3 targetDirection = closest.transform.position - transform.position;

                objectPool.GetNextObject(transform.position, Quaternion.LookRotation(targetDirection));
            }
        }
    }

}
