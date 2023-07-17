using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MemberController : MonoBehaviour
{
    public static MemberController instance;
    public GameObject player;


    private void Awake()
    {
        if(instance != null)
        {
            DestroyImmediate(instance);
            return ;
        } else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AddMember();
        }
    }

    public void AddMember()
    {
        Member1[] members = GetComponentsInChildren<Member1>();

        GameObject memberObject = new GameObject("Member");
        memberObject.transform.parent = transform;
        Member1 member = memberObject.AddComponent<Member1>();

        GameObject followTarget = (members.Length == 0 ? player : members[members.Length - 1].gameObject);

        member.SetCharacterInfo(DataManager.instance.characterList[0], followTarget);
    }
}
