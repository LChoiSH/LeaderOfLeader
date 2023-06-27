using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessMan : Leader
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Skill()
    {
        base.Skill();

        StartCoroutine(FarmerSkill());
    }

    protected virtual IEnumerator FarmerSkill()
    {

        float currentAttackSpeed = attackSpeed;
        attackSpeed /= 10;

        yield return new WaitForSeconds(5.0f);

        attackSpeed = currentAttackSpeed;
    }
}
