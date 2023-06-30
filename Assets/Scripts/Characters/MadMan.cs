using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadMan : Leader
{
    public override void DoSkill()
    {
        if (currentSkillTime > 0) return;

        base.DoSkill();

        StartCoroutine(MadSkill());
    }

    private IEnumerator MadSkill()
    {
        float currentSpeed = speed;
        speed *= 3;

        yield return new WaitForSeconds(3.0f);

        speed = currentSpeed;
    }
}
