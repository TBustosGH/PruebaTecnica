using UnityEngine;
using System.Linq;

public class enemySpawnController : MonoBehaviour
{
    [Header("Spawn Configurations")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float timeBeetwenEnemies;
    [SerializeField] private GameObject player;
    private float minX, maxX, minY, maxY, timeSinceLastEnemy;

    private void Start()
    {
        minX = spawnPoints.Min(spawnPoint => spawnPoint.position.x);
        maxX = spawnPoints.Max(spawnPoint => spawnPoint.position.x);
        minY = spawnPoints.Min(spawnPoint => spawnPoint.position.y);
        maxY = spawnPoints.Max(spawnPoint => spawnPoint.position.y);
    }

    private void Update()
    {
        timeSinceLastEnemy += Time.deltaTime;

        if (timeSinceLastEnemy >= timeBeetwenEnemies)
        {
            timeSinceLastEnemy = 0;
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        int enemyType = Random.Range(0, enemies.Length);
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        enemies[enemyType].GetComponent<enemyController>().player = player;
        Instantiate(enemies[enemyType], randomPosition, Quaternion.identity);
    }
}
