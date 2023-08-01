using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    override protected void Attack()
    {
        if(Vector3.Distance(transform.position, target.transform.position) > attackDistance)
        {
            attackDelayTime = attackTime;

            SetState(EnemyState.Move);
        } 
        else
        {
            attackDelayTime += Time.deltaTime;
            
            if(attackDelayTime >= attackTime)
            {
                transform.LookAt(target.transform.position);
                animator.SetTrigger("StartAttack");
                attackDelayTime = 0;
            }
        }
    }
}
