using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadMan : Leader
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Skill()
    {
        base.Skill();

        StartCoroutine(MadSkill());
    }

    protected virtual IEnumerator MadSkill()
    {

        float currentSpeed = speed;
        speed *= 3;

        yield return new WaitForSeconds(5.0f);

        speed = currentSpeed;
    }
}
