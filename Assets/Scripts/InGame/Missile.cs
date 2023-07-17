using System.Collections;
using System.Collections.Generic;
//using Unity.Notifications.Android;
using UnityEngine;

public class Missile : MonoBehaviour, Attackable
{
    float flyingTime = 5.0f;

    public float speed = 10.0f;
    [SerializeField] ParticleSystem particle;
    [SerializeField] int damage = 100;

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
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Prison"))
        {
            Attack(other.GetComponent<Damageable>());
        }
    }

    public void Attack(Damageable damageable)
    {
        Instantiate(particle, transform.position, transform.rotation);
        damageable.Damaged(damage);
        gameObject.SetActive(false);
    }

    IEnumerator HideSelf()
    {
        yield return new WaitForSeconds(flyingTime);
        transform.gameObject.SetActive(false);
    }
}
