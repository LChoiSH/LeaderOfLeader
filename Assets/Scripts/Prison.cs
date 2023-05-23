using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Prison : MonoBehaviour
{
    private CharacterInfo characterInfo;
    private int hp = 100;
    GameObject characterPrefab;
    MemberController memberController;
    GameObject characterObject;

    // Start is called before the first frame update
    void Start()
    {
        memberController = GameObject.Find("Member Controller").GetComponent<MemberController>();

        // character info
        int length = DataManager.instance.characterDictionary.Count;

        characterInfo = DataManager.instance.characterDictionary.ElementAt(Random.Range(0, length)).Value;
        string prefabPath = "Prefabs/Character/" + characterInfo.prefab;

        // get character prefab
        characterPrefab = Resources.Load<GameObject>(prefabPath);

        characterObject = Instantiate(characterPrefab, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    public void Damaged(int damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            Member myMember = memberController.addMember();

            if (myMember != null)
            {
                myMember.SetCharacterInfo(characterInfo);
                Destroy(characterObject);
                Destroy(gameObject);
            }
        } 
    }
}
