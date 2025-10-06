using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public int difficulty;

    private int[,] enemySpawns =
    {
        { 3, 1, 0, 0, 0 },
        { 5, 7, 0, 0, 0 },
        { 5, 4, 3, 0, 0 },
        { 8, 8, 2, 3, 0 },
        { 8, 9, 4, 2, 1 }
    };

    public GameObject[] enemies;

    private void Awake()
    {
        Spawn();
    }

    public void Spawn()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < enemySpawns[difficulty, i];  j++)
            {
                Vector3 spawnPos = transform.position;
                GameObject enemy = Instantiate(enemies[i], spawnPos, Quaternion.identity);

                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 randomDir = Random.insideUnitCircle.normalized;
                    float randomSpeed = Random.Range(4, 20);
                    rb.linearVelocity = randomDir * randomSpeed;
                }
            }
        }
    }
}