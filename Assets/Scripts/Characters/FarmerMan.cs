using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FarmerMan : Leader
{
    GameObject fruitPrefab;

    protected override void Start()
    {
        base.Start();

        string fruitPath = "Prefabs/etc/Farmer_Fruit";
        fruitPrefab = Resources.Load<GameObject>(fruitPath);
    }

    protected override void Skill()
    {
        List<Member> members = MemberController.instance.GetMembers();

        foreach(Member member in members)
        {
            Vector3 fruitPosition = transform.position;
            fruitPosition.y += 1;
            FarmerRecoveryFruit farmerRecoveryFruit = Instantiate(fruitPrefab, fruitPosition, transform.rotation).GetComponent<FarmerRecoveryFruit>();

            farmerRecoveryFruit.SetRecovery(member.gameObject, 2);
        }
    }
}
