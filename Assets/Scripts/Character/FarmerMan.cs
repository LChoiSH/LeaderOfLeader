using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerMan : Leader
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Skill()
    {
        base.Skill();
        FarmerSkill();
    }

    protected virtual void FarmerSkill()
    {
        currentHp = maxHp;
        healthBar.SetHealth(currentHp);
    }
}
