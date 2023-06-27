using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MemberController : MonoBehaviour
{
    public Member[] members;
    public GameObject hpBar;

    public Member addMember()
    {
        for(int i = 0; i < members.Length;i++)
        {
            if (members[i].gameObject.activeSelf == false)
            {
                members[i].gameObject.SetActive(true);
                return members[i];
            }
        }

        return null;
    }
}
