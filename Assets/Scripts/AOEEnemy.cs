using UnityEngine;

public class AOEEnemy : Damagable
{
    [SerializeField] private float strafeDistance = 10f;
    private Renderer spriteRenderer;
    private Camera mainCamera;
    public int speed;
    private Animator animator;
    private int _attackAnimTimer;


    [SerializeField] private int aoeCooldown = 200;
    [SerializeField] private int aoeMeteors = 5;
    [SerializeField] private GameObject meteorPrefab;
    private bool inRange;
    private int aoeTimer = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<Renderer>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (IsSpriteOnScreen() && _attackAnimTimer == 0)
        {
            animator.SetBool("IsAttacking", false);
            MoveTowardPlayer();
        }
    }

    private void FixedUpdate()
    {
        aoeTimer = Mathf.Max(aoeTimer - 1, 0);
        _attackAnimTimer = Mathf.Max(_attackAnimTimer - 1, 0);
        if (aoeTimer == 0 && inRange)
        {
            aoeTimer = aoeCooldown;
            _attackAnimTimer = 100;
        }
        if (aoeTimer >= aoeCooldown - aoeMeteors * 4 && aoeTimer % 4 == 0)
        {
            animator.SetBool("IsAttacking", true);

            Instantiate(meteorPrefab, PlayerStats.Instance.playerTransform + (Vector3)(Random.insideUnitCircle * 3f), Quaternion.identity);
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

        if (desiredVelocity.x != 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (desiredVelocity.x < 0 ? 1 : -1);
            transform.localScale = scale;
        }
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
