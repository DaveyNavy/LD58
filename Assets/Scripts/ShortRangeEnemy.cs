using System.Collections;
using UnityEngine;

public class ShortRangeEnemy : Enemy
{
    public GameObject squarePrefab;
    public GameObject windupPrefab;

    public float spawnDistance = 2f;

    [SerializeField] private int AttackCD;
    private int _attackTimer;
    private int _attackAnimTimer;

    private int _attackRange = 3;

    private Rigidbody2D rb;

    private int originalSpeed;

    protected override void Awake()
    {
        base.Awake();
        originalSpeed = speed;
    }

    protected override void Update()
    {
        base.Update();
        rb = GetComponent<Rigidbody2D>();
        if (_attackAnimTimer <= 0)
        {
            speed = originalSpeed;
        }

        Attack();
    }

    private void FixedUpdate()
    {
        _attackTimer = Mathf.Max(0, _attackTimer - 1);
        _attackAnimTimer = Mathf.Max(0, _attackAnimTimer - 1);
    }

    public void Attack()
    {
        if (_attackTimer > 0) return;
        _attackTimer = AttackCD;

        if (Vector3.Distance(transform.position, PlayerStats.Instance.playerTransform) > _attackRange)
        {
            return;
        }

        StartCoroutine(SpawnWindupThenAttack());
    }

    private IEnumerator SpawnWindupThenAttack()
    {
        Vector3 directionToPlayer = (PlayerStats.Instance.playerTransform - transform.position).normalized;

        Vector3 spawnPos = transform.position + directionToPlayer * spawnDistance;

        speed = 0;
        _attackAnimTimer = 50;

        if (StunTimer > 0.1f) yield break;

        // Calculate rotation that faces the player (2D)
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        // Spawn windup facing the player
        Instantiate(windupPrefab, spawnPos, rotation);

        for (int i = 0; i < 40; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        if (StunTimer > 0.1f) yield break;

        Instantiate(squarePrefab, spawnPos, rotation);
    }
}
