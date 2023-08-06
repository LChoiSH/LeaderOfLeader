using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beholder : Enemy
{

    public BeholderLaser laser;
    public bool isAttackFixed;
    public float attackingTime;

    protected override void Start()
    {
        base.Start();
        laser.gameObject.SetActive(false);
        isAttackFixed = false;
        attackDelayTime = 0;
    }

    override protected void Attack()
    {
        if (!isAttackFixed)
        {
            isAttackFixed = true;
            transform.LookAt(target.transform.position);
            return;
        }

        attackDelayTime += Time.deltaTime;

        // 공격 준비 중
        if (attackDelayTime < attackTime) return;
        
        // 공격 끝
        if (attackingTime + attackTime < attackDelayTime) 
        {
            animator.SetTrigger("AttackToMove");
            animator.SetBool("isAttack", false);
            attackDelayTime = 0;
            isAttackFixed = false;
            laser.gameObject.SetActive(false);
            SetState(EnemyState.Move);
            return;
        }

        // 공격
        if (attackDelayTime + 0.5 > attackTime)
        {
            animator.SetBool("isAttack", true);
            laser.gameObject.SetActive(true);
        }
    }

    public override void Damaged(int damage)
    {
        currentHp -= damage;
        healthBar.SetHealth(currentHp);

        if (currentHp <= 0)
        {
            SetState(EnemyState.Die);
            gameObject.tag = "Untagged";
            animator.SetTrigger("Die");
            Die();
        }
        else if (currentState != EnemyState.Attack)
        {
            SetState(EnemyState.Hit);
            animator.SetTrigger("Hit");
        }
    }

    protected override void Die()
    {
        laser.gameObject.SetActive(false);

        base.Die();
    }
}
