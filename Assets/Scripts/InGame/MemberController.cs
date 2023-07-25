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
            Destroy(gameObject);
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.timeScale == 0)
            {
                Time.timeScale = 1;
            } else
            {
                Time.timeScale = 0;
            }
        }
    }

    public void AddMember()
    {
        Member[] members = GetComponentsInChildren<Member>();

        GameObject memberObject = new GameObject("Member");
        memberObject.transform.parent = transform;
        Member member = memberObject.AddComponent<Member>();

        GameObject followTarget = (members.Length == 0 ? player : members[members.Length - 1].gameObject);

        member.SetCharacterInfo(DataManager.instance.characterList[Random.Range(0, DataManager.instance.characterList.Length)], followTarget);
    }
}
