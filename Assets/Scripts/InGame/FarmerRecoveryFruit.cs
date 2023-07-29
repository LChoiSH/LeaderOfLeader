using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FarmerRecoveryFruit : MonoBehaviour
{
    Rigidbody selfRb;
    public GameObject target;
    public GameObject fruit;
    public GameObject recoveryEffect;

    float speed = 5.0f;
    float forcePower = 100f;
    float torquePower = 1000f;

    Vector3 targetPosition, currentPosition, direction;

    bool isArrive = false;
    bool isEat = false;

    int recovery = 20;

    void Start()
    {
        selfRb = GetComponent<Rigidbody>();

        float randomX = Random.Range(0f, 360f);
        float randomY = Random.Range(0f, 360f);
        float randomZ = Random.Range(0f, 360f);

        Quaternion randomRotation = Quaternion.Euler(randomX, randomY, randomZ);

        transform.rotation = randomRotation;

        selfRb.AddForce(Vector3.up * forcePower, ForceMode.Force);
        selfRb.AddTorque(Random.onUnitSphere * torquePower, ForceMode.Force);
    }

    void FixedUpdate()
    {
        if (target == null) Destroy(gameObject);

        targetPosition = target.transform.position;
        currentPosition = transform.position;

        if(isEat)
        {
            transform.position = new Vector3(targetPosition.x, currentPosition.y, targetPosition.z);
        }
        else if (isArrive)
        {
            transform.position = new Vector3(targetPosition.x, currentPosition.y, targetPosition.z);
            selfRb.AddForce(-Vector3.up * 10, ForceMode.Acceleration);
        }
        else
        {
            targetPosition.y = 0;
            currentPosition.y = 0;

            direction = (targetPosition - currentPosition).normalized;

            transform.position += (direction * speed * Time.deltaTime);
            transform.position += (Vector3.up * speed * Time.deltaTime);

            if ((targetPosition - currentPosition).magnitude < 0.1f) isArrive = true;
        }
    }

    public void SetRecovery(GameObject target, int recovery)
    {
        this.target = target;
        this.recovery = recovery;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent != null && other.transform.parent.gameObject == target)
        {
            isEat = true;
            selfRb.velocity = Vector3.zero;
            fruit.SetActive(false);
            StartCoroutine(Recovery());
        }
    }

    IEnumerator Recovery()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

        Member member = target.GetComponentInChildren<Member>();
        member.RecoveryHp(recovery);

        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }

        yield return new WaitForSeconds(particles[particles.Length - 1].duration);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Destroy(recoveryEffect);
    }
}
