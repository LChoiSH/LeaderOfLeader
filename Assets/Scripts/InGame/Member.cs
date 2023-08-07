using System.Collections;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Member : MonoBehaviour, Damageable

{
    bool isLoading = true;

    // move
    public Member followingTarget;

    // prefab
    CharacterInfo myCharacterInfo;
    public GameController gameController;

    // attack
    [SerializeField] float attackSpeed;
    [SerializeField] float currentAttackSpeed;
    [SerializeField] float attackTime;
    [SerializeField] float attackDelayTime;
    private ObjectPool<Missile> missileObjectPool;
    private GameObject[] attackTargetArray;
    private float targetDistance, closestTargetDistance;
    private float attackRange = 50.0f;

    // damage
    private int damage;
    private int currentDamage;

    // damaged
    int maxHp = 100;
    [SerializeField] int currentHp = 100;
    bool isDamaged = false;
    float hitTime = 1.5f;
    HealthBar healthBar;
    int armor;
    AudioSource damagedAudio;

    // animation
    Animator animator;

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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Damaged(9999);
        }

        if (isLoading) return;
        Move();
    }

    #nullable enable
    public void SetCharacterInfo (CharacterInfo characterInfo, Member? followingTarget)
    {
        myCharacterInfo= characterInfo;

        // attack Speed Setting
        attackSpeed = characterInfo.attackSpeed;
        currentAttackSpeed = attackSpeed;
        attackTime = 100 / attackSpeed;
        attackDelayTime = 0;

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

        // damaged Setting;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        armor = 0;
        damagedAudio = gameObject.AddComponent<AudioSource>();
        damagedAudio.playOnAwake= false;
        string audioClipPath = "Audio/" + DataManager.instance.characterDictionary[myCharacterInfo.id].damagedSound;
        AudioClip myAudioClip = Resources.Load<AudioClip>(audioClipPath);
        damagedAudio.clip = myAudioClip;

        // damage Setting
        damage = characterInfo.attackDamage;
        currentDamage = damage;

        // Missile Setting
        string missilePrefabPath = "Prefabs/Missile/" + DataManager.instance.characterDictionary[myCharacterInfo.id].missilePrefab;
        Missile missile = Resources.Load<GameObject>(missilePrefabPath).GetComponent<Missile>();
        missile.SetAttacker(this);

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

        gameObject.tag = "Member";

        isLoading = false;
    }

    public virtual void Damaged(int damage)
    {
        if (!GameController.instance.isGameActive) return;

        damage = Mathf.Clamp(damage - armor, 0, currentHp);

        if (!isDamaged && currentHp > 0)
        {
            if(damagedAudio != null) { damagedAudio.Play(); }

            currentHp -= damage;
            healthBar.SetHealth(currentHp);
            isDamaged = true;
            StartCoroutine(SetInvincible(hitTime));

            if (currentHp <= 0)
            {
                currentHp = 0;
                animator.SetTrigger("TriggerDeath");
                transform.parent = null;

                if (GetComponent<MovePlayer>() != null)
                {
                    GameController.instance.GameOver();
                }
                else
                {
                    Invoke("Die", 5.0f);
                }
            }
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
        if(followingTarget.currentHp <= 0)
        {
            Damaged(999999);
            yield break;
        }

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
            attackDelayTime += Time.deltaTime;
            if (attackTime > attackDelayTime)
            {
                yield return null;
                continue;
            }
            attackDelayTime = 0;

            attackTargetArray = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject? closestTarget = null;
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

    public int GetCurrentHp()
    {
        return currentHp;
    }

    public void RecoveryHp(int value)
    {
        currentHp += value;
        if (currentHp > maxHp) currentHp = maxHp;

        healthBar.SetHealth(currentHp);
    }

    public void AttackSpeedUp(float value)
    {
        currentAttackSpeed += value;

        if(currentAttackSpeed <= 0)
        {
            attackTime = float.MaxValue;
        } else
        {
            attackTime = 100 / currentAttackSpeed;
        }
    }

    public int GetDamage()
    {
        return currentDamage;
    }

    public void DamageUp(int value)
    {
        currentDamage += value;
    }

    public void DamageUp(float x) {
        if (x <= 0) return;

        currentDamage = currentDamage + (int)(damage * x);
    }

    public void ArmorUp(int x)
    {
        armor += x;
    }
}
