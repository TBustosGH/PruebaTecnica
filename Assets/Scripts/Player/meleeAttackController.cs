using System;
using UnityEngine;

public class meleeAttackController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<enemyController>().BeingAttacked(GetComponentInParent<playerController>().playerDamage, transform.position);
        }
    }
}
