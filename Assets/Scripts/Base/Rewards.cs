using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RewardEnum { InputMember, HealCharacters, c };

public class Rewards : MonoBehaviour
{
    string[] rewardsList;

    // Start is called before the first frame update
    void Start()
    {
        rewardsList = Enum.GetNames(typeof(RewardEnum));
    }

    void RandomRewards()
    {
        while(true)
        {
            int randomIndex = UnityEngine.Random.Range(0, rewardsList.Length);
            Invoke(rewardsList[randomIndex], 0);
        }
    }

    void InputMember()
    {
        Debug.Log("inputMember");
    }

    void HealCharacters()
    {
        Debug.Log("inputMember");
        //GameObject.Find("Members");
    }
}
