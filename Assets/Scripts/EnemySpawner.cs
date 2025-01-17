using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CannonPlayer;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRange;
    private float spawnTime;
    public float spawnInterval;
    public GameObject explosionPrefab;
    public GameObject enemyPrefab;
    private CannonPlayer cannonPlayer;  // Reference to the CannonPlayer script

    private void Start()
    {
        // Find and reference the CannonPlayer script
        cannonPlayer = Object.FindFirstObjectByType<CannonPlayer>();
        if (cannonPlayer == null)
        {
            Debug.LogError("CannonPlayer script not found! Make sure the player object is correctly set up.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }

    void Update()
    {
        // Check if the game state is Playing before spawning enemies
        if (cannonPlayer != null && cannonPlayer.playerState == CannonPlayer.PlayerState.Playing)
        {
            spawnTime += Time.deltaTime;
            if (spawnTime > spawnInterval)
            {
                spawnTime = 0;
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab);

        // Calculate a random position within spawnRange
        float randomX = Random.Range(-spawnRange, spawnRange);
        float randomZ = Random.Range(-spawnRange, spawnRange);

        // Set Y to a fixed value (5.0f)
        Vector3 spawnPos = new Vector3(transform.position.x + randomX, 5.0f, transform.position.z + randomZ);

        // Set the enemy's position to the calculated spawn position
        enemy.transform.position = spawnPos;
    }
}
