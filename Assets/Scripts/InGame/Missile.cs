using System.Collections;
using System.Collections.Generic;
//using Unity.Notifications.Android;
using UnityEngine;

public class Missile : MonoBehaviour, Attackable
{
    public Member attacker;
    float flyingTime = 5.0f;

    public float speed = 10.0f;
    [SerializeField] ParticleSystem particle;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        StartCoroutine(HideSelf());
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (attacker == null) Destroy(gameObject);

        if (other.CompareTag("Environment"))
        {
            if (GetComponent<AudioSource>() != null) GetComponent<AudioSource>().Play();
            gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            if (GetComponent<AudioSource>() != null) GetComponent<AudioSource>().Play();
            Attack(other.GetComponent<Damageable>());
        }
    }

    public void Attack(Damageable damageable)
    {
        if(particle != null)
        {
            Instantiate(particle, transform.position, transform.rotation);
        }

        if (attacker != null)
        {
            damageable.Damaged(attacker.GetDamage());
        }

        gameObject.SetActive(false);
    }

    IEnumerator HideSelf()
    {
        yield return new WaitForSeconds(flyingTime);
        transform.gameObject.SetActive(false);
    }

    public void SetAttacker(Member member)
    {
        attacker = member;
    }
}
