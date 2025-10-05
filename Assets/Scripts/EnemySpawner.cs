using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;       // Enemy prefab to spawn
    public Transform[] spawnPoints;      // Positions to spawn from
    public int enemiesPerWave = 5;       // How many enemies per wave
    public float timeBetweenEnemies = 1f;
    public float timeBetweenWaves = 5f;
    public int totalWaves = 3;

    private int currentWave = 0;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < totalWaves)
        {
            currentWave++;
            Debug.Log($"Starting Wave {currentWave}");

            yield return StartCoroutine(SpawnSingleWave());

            Debug.Log($"Wave {currentWave} complete");
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("All waves complete!");
    }

    IEnumerator SpawnSingleWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    void SpawnEnemy()
    {
        // Pick a random spawn point if multiple
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}