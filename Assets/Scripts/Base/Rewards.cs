using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RewardEnum { InputMember, HealCharacters, c };

public class Rewards
{
    string[] rewardsList;

    // Start is called before the first frame update
    //void Start()
    //{
    //    rewardsList = Enum.GetNames(typeof(RewardEnum));
    //}

    //void RandomRewards()
    //{
    //    while(true)
    //    {
    //        int randomIndex = UnityEngine.Random.Range(0, rewardsList.Length);
    //        Invoke(rewardsList[randomIndex], 0);
    //    }
    //}

    public static void AddMember()
    {
        MemberController.instance.AddMember();
    }

    public static void HealCharacters()
    {
        Debug.Log("HealCharacters");
    }

    public static void CancelReward()
    {
        Debug.Log("CancelReward");
    }
}
