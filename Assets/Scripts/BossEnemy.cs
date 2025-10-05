using UnityEngine;

public class BossEnemy : Damagable
{
    [SerializeField] private float strafeDistance = 3f;
    private Renderer spriteRenderer;
    private Camera mainCamera;
    public int speed;

    [SerializeField] private int aoeCooldown = 200;
    [SerializeField] private GameObject bossAttackPrefab;
    [SerializeField] private GameObject bossAttack2Prefab;
    private bool inRange;
    private int aoeTimer = 0;

    void Start()
    {
        spriteRenderer = GetComponent<Renderer>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (IsSpriteOnScreen())
        {
            MoveTowardPlayer();
        }
    }

    private void FixedUpdate()
    {
        aoeTimer = Mathf.Max(aoeTimer - 1, 0);
        if (aoeTimer == 0 && inRange)
        {
            Vector3 directionToPlayer = (PlayerStats.Instance.playerTransform - transform.position).normalized;
            Vector3 spawnPos = transform.position + directionToPlayer * 4f;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg + 90f;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            Instantiate(bossAttackPrefab, spawnPos, rotation);

            aoeTimer = aoeCooldown;
        }
        if (aoeTimer == 0 && !inRange)
        {
            Instantiate(bossAttack2Prefab, transform.position, Quaternion.identity, transform);

            aoeTimer = aoeCooldown;
        }
    }

    #region Effects
    public float StunTimer;

    #endregion Effects

    void MoveTowardPlayer()
    {
        if (StunTimer > 0.1f)
        {
            StunTimer -= Time.deltaTime;
            return;
        }
        // Move toward player if far, move away if close:
        Vector2 toPlayer = PlayerStats.Instance.playerTransform - transform.position;
        int multiplier = (toPlayer.magnitude < strafeDistance) ?  -1 : 1;
        inRange = (toPlayer.magnitude < strafeDistance + 2f);

        Vector2 direction = toPlayer.normalized;
        Vector2 desiredVelocity = direction * speed * multiplier;
        Vector2 currentVelocity = _rb.linearVelocity;
        Vector2 velocityChange = desiredVelocity - currentVelocity;
        _rb.AddForce(velocityChange, ForceMode2D.Force);
    }

    bool IsSpriteOnScreen()
    {
        if (spriteRenderer == null || mainCamera == null)
        {
            return false;
        }

        if (!spriteRenderer.isVisible)
        {
            return false;
        }

        Vector3 screenPoint = mainCamera.WorldToScreenPoint(spriteRenderer.bounds.center);

        bool onScreenX = screenPoint.x > 0 && screenPoint.x < Screen.width;
        bool onScreenY = screenPoint.y > 0 && screenPoint.y < Screen.height;
        bool inFrontOfCamera = screenPoint.z > 0;

        return onScreenX && onScreenY && inFrontOfCamera;
    }
}
