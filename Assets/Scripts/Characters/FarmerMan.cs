using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerMan : Leader
{
    public override void DoSkill()
    {
        base.DoSkill();
        FarmerSkill();
    }

    private void FarmerSkill()
    {
        currentHp = maxHp;
        healthBar.SetHealth(currentHp);
    }
}
