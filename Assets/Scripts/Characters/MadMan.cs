using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadMan : Leader
{
    public override void DoSkill()
    {
        base.DoSkill();

        StartCoroutine(MadSkill());
    }

    private IEnumerator MadSkill()
    {

        float currentSpeed = speed;
        speed *= 3;

        yield return new WaitForSeconds(5.0f);

        speed = currentSpeed;
    }
}
