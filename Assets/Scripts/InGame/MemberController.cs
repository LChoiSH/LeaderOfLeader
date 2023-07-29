using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MemberController : MonoBehaviour
{
    public static MemberController instance;
    public static Vector3 hidePosition = new Vector3(0, -20, 0);

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return ;
        } else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            AddMember();
        }
    }

    public void AddMember()
    {
        Member[] members = GetComponentsInChildren<Member>();

        GameObject memberObject = new GameObject("Member");
        memberObject.transform.parent = transform;
        Member member = memberObject.AddComponent<Member>();
        member.transform.position = hidePosition;
        Member followTarget = (members.Length == 0 ? GameObject.Find("Player").GetComponent<Member>() : members[members.Length - 1]);

        member.SetCharacterInfo(DataManager.instance.characterList[Random.Range(0, DataManager.instance.characterList.Length)], followTarget);
    }

    public List<Member> GetMembers()
    {
        List<Member> members = GetComponentsInChildren<Member>().ToList();

        return members;
    }

    public void MemberHide()
    {
        Member[] members = GetComponentsInChildren<Member>();
        
        foreach (Member member in members)
        {
            member.transform.position = hidePosition;
        }
    }
}
