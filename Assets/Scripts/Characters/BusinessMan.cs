using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessMan : Leader
{
    Member member;
    BoxCollider selfCollider;
    ParticleSystem fireParticle;

    void Start()
    {
        member = GetComponent<Member>();

        string fireParticlePath = "Prefabs/Particle/FX_BusinessFire";
        GameObject fireParticlePrefab = Resources.Load<GameObject>(fireParticlePath);
        fireParticle = Instantiate(fireParticlePrefab, transform).GetComponent<ParticleSystem>();
        fireParticle.Stop();
        fireParticle.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }

    protected override void Skill()
    {
        BoxCollider selfCollider = GetComponentInChildren<BoxCollider>();
        float height = selfCollider.size.y;
        fireParticle.transform.localPosition = new Vector3(0, height, 0);
        StartCoroutine(BusinessFire());
    }

    IEnumerator BusinessFire()
    {
        fireParticle.Play();
        member.AttackSpeedUp(200);

        float currentTime = 0;
        while(currentTime < 5)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        member.AttackSpeedUp(-200);
        fireParticle.Stop(false, ParticleSystemStopBehavior.StopEmitting);

    }
}
