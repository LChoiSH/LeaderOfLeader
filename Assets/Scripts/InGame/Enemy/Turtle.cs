using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enemy
{
    public GameObject turtleMissile;

    override protected void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
        {
            attackDelayTime = attackTime;

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
        transform.LookAt(target.transform.position);

        GameObject missile = Instantiate(turtleMissile);
        missile.transform.position = transform.position;
        missile.transform.LookAt(target.transform.position);
        missile.GetComponent<EnemyMissile>().SetAttacker(this);
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(3);

        Destroy(turtleMissile);
    }
}
