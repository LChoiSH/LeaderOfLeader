using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadMan : Leader
{
    MovePlayer movePlayer;
    ParticleSystem madRunParticle;

    void Start()
    {
        movePlayer= GetComponent<MovePlayer>();

        string runParticlePath = "Prefabs/Particle/FX_MadRun";
        GameObject runParticlePrefab = Resources.Load<GameObject>(runParticlePath);
        madRunParticle = Instantiate(runParticlePrefab, transform).GetComponent<ParticleSystem>();
        madRunParticle.Stop();
    }

    protected override void Skill()
    {
        StartCoroutine(MadRun());
    }

    IEnumerator MadRun()
    {
        madRunParticle.Play();
        movePlayer.speed *= 2;
        yield return new WaitForSeconds(3);
        movePlayer.speed /= 2;
        madRunParticle.Stop();
    }
}
