using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerMan : Leader
{
    public override void DoSkill()
    {
        if (currentSkillTime > 0) return;
        
        base.DoSkill();

        FarmerSkill();
    }

    private void FarmerSkill()
    {
        currentHp = maxHp;
        healthBar.SetHealth(currentHp);
    }
}
