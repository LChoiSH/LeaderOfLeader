using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessMan : Leader
{
    protected override void Skill()
    {
        Debug.Log("Business Man Skill");
    }

    //public override void DoSkill()
    //{
    //    if (currentSkillTime > 0) return;

    //    base.DoSkill();

    //    StartCoroutine(FarmerSkill());
    //}

    //private IEnumerator FarmerSkill()
    //{

    //    //float currentAttackSpeed = attackSpeed;
    //    //attackSpeed /= 10;

    //    yield return new WaitForSeconds(5.0f);

    //    //attackSpeed = currentAttackSpeed;
    //}
}
