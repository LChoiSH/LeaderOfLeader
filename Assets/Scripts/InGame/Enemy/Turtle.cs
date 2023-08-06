using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enemy
{
    public GameObject turtleMissile;

    override protected void Attack()
    {
        transform.LookAt(target.transform.position);

        if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
        {
            SetState(EnemyState.Move);
        }
        else
        {
            attackDelayTime += Time.deltaTime;

            if (attackDelayTime >= attackTime)
            {
                animator.SetTrigger("StartAttack");
                attackDelayTime = 0;
                Fire();
            }
        }
    }

    void Fire()
    {
        GameObject missile = Instantiate(turtleMissile);
        missile.transform.position = transform.position;
        missile.transform.LookAt(target.transform.position);
        missile.GetComponent<EnemyMissile>().SetAttacker(this);
    }
}
