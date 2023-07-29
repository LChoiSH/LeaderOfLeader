using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Rewards
{
    string[] rewardsList;

    public static void AddMember()
    {
        MemberController.instance.AddMember();
    }

    public static void HealCharacters()
    {
        List<Member> members = MemberController.instance.GetMembers();

        foreach (Member member in members)
        {
            member.RecoveryHp(10000);
        }
    }

    public static void AddDamage()
    {
        List<Member> members = MemberController.instance.GetMembers();

        foreach(Member member in members)
        {
            member.DamageUp(20);
        }
    }

    public static void AddArmor()
    {
        List<Member> members = MemberController.instance.GetMembers();

        foreach (Member member in members)
        {
            member.ArmorUp(10);
        }
    }

    public static void AddAttackSpeed()
    {
        List<Member> members = MemberController.instance.GetMembers();

        foreach (Member member in members)
        {
            member.AttackSpeedUp(20);
        }
    }
}
