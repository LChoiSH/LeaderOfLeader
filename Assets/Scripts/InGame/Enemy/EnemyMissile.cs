using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    public Enemy attacker;
    float flyingTime = 5.0f;
    public float speed = 10.0f;

    [SerializeField] ParticleSystem particle;

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
        if(attacker == null) Destroy(gameObject);

        if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }

        try
        {
            if (other.CompareTag("Member"))
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

    public void Attack(Damageable damageable)
    {
        if (particle != null)
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

    public void SetAttacker(Enemy enemy)
    {
        attacker = enemy;
    }
}
