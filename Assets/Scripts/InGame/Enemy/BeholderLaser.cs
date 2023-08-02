using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BeholderLaser : MonoBehaviour
{
    public Enemy attacker;

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            if (other.CompareTag("Member"))
            {
                Attack(other.GetComponent<Damageable>());
            }
            else if (other.transform.parent != null && other.transform.parent.CompareTag("Member"))
            {
                Attack(other.GetComponentInParent<Damageable>());
            }
        }
        catch
        {

        }
    }

    public void Attack(Damageable damageable)
    {
        if (attacker != null)
        {
            damageable.Damaged(attacker.GetDamage());
        }
    }
}
