using Unity.VisualScripting;
using UnityEngine;

public class AttackerController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int enemyDamage;
    public int enemyHealtPoints;
    private bool isEnemyAlive = true;

    private void Update()
    {
        if (isEnemyAlive)
        {
            if (enemyHealtPoints <= 0)
                isEnemyAlive = false;
            else
                isEnemyAlive = true;
        }
        else
            transform.Translate(Vector3.up);
    }
    public void BeingAttacked(int damage)
    {
        if (enemyHealtPoints > damage)
        {
            enemyHealtPoints -= damage;
        }
        else
        {
            enemyHealtPoints = 0;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<playerController>().BeingAttacked(enemyDamage);
        }
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player Attack"))
        {
            BeingAttacked(other.GetComponent<playerController>().playerDamage);
        }
    }*/
}
