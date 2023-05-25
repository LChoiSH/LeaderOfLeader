using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MemberController : MonoBehaviour
{
    private GameObject player;
    public Member[] members;
    public GameObject hpBar;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

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
