using UnityEngine;

public class enemyController : MonoBehaviour
{
    [Header("Enemy Configuration")]
    public int enemyDamage = 15;
    public int enemyHealtPoints = 150;
    public float enemySpeed, enemyDetectionArea, hitForce;
    public GameObject player;
    private bool isBeingAttacked, isEnemyAlive, isPlayerAlive;
    private int rutine;
    private float timer, grado, chillingTime;
    private Quaternion angle;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        isPlayerAlive = player.GetComponent<playerController>().isPlayerAlive;
        //Check hp
        if (enemyHealtPoints > 0)
            isEnemyAlive = true;
        else
            isEnemyAlive = false;

        //Control behaviour
        if (isEnemyAlive)
        {
            EnemyBehaviour();
            StopBeingAttacked();
        }
    }

    private void EnemyBehaviour()
    {
        if (isPlayerAlive)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > enemyDetectionArea)
            {
                Roaming();
            }
            else
            {
                FollowPlayer();
            }
        }
        else
            Roaming();
    }

    private void FollowPlayer()
    {
        var lookPos = player.transform.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);

        transform.Translate(Vector3.forward * (enemySpeed * 3) * Time.deltaTime);
    }
    private void Roaming()
    {
        timer += Time.deltaTime;
        if (timer >= 4)
        {
            timer = 0;
            rutine = Random.Range(0, 2);
        }
        switch (rutine)
        {
            case 0:
                //stay in place
                float velocityY = rb.linearVelocity.y;
                transform.Translate(new Vector3(0, velocityY, 0));
                break;
            case 1:
                //Rotate to some new direction
                grado = Random.Range(0, 360);
                angle = Quaternion.Euler(0, grado, 0);
                rutine++;
                break;
            case 2:
                //Move to some new position
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, 0.5f);
                transform.Translate(Vector3.forward * enemySpeed * Time.deltaTime);
                break;
        }
    }

    public void BeingAttacked(int damage, Vector3 hitDirection)
    {
        if (!isBeingAttacked)
        {
            if (enemyHealtPoints > damage)
            {
                enemyHealtPoints -= damage;
                chillingTime = 0;
                isBeingAttacked = true;
                rb.AddForce(hitDirection * hitForce * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                enemyHealtPoints = 0;
                isEnemyAlive = false;
                isBeingAttacked = false;
            }
        }
    }
    public void StopBeingAttacked()
    {
        if (isBeingAttacked && enemyHealtPoints > 0)
        {
            chillingTime += Time.deltaTime;
            if (chillingTime >= 0.2f)
            {
                chillingTime = 0;
                isBeingAttacked = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isEnemyAlive) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<playerController>().BeingAttacked(enemyDamage);
        }
    }
}
