using System.Collections;
using System.Collections.Generic;
//using Unity.Notifications.Android;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 10.0f;
    [SerializeField] ParticleSystem particle;
    [SerializeField] int damage = 100;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(particle, other.transform.position, transform.rotation);
            other.GetComponent<Enemy>().Damaged(damage);
            gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Prison"))
        {
            Instantiate(particle, other.transform.position, transform.rotation);
            other.GetComponent<Prison>().Damaged(damage);
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Remove());
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }

}
