using UnityEngine;
using System.Collections;

public class ChargingEnemy : Enemy
{
    private int _attackTimer;
    private int _attackRange = 20;
    private int _attackAnimTimer = 500;
    [SerializeField] private int AttackCD;

    private int originalSpeed;

    private Coroutine Coroutine;

    private LineRenderer lineRenderer;
    private LineRenderer lineRenderer2;

    protected override void Start()
    {
        base.Start();
        // Create child GameObject for first LineRenderer
        GameObject lineObj1 = new GameObject("ChargeLine1");
        lineObj1.transform.parent = transform;
        lineObj1.transform.localPosition = Vector3.zero;
        lineRenderer = lineObj1.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.enabled = false;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.sortingOrder = 10;

        // Create child GameObject for second LineRenderer
        GameObject lineObj2 = new GameObject("ChargeLine2");
        lineObj2.transform.parent = transform;
        lineObj2.transform.localPosition = Vector3.zero;
        lineRenderer2 = lineObj2.AddComponent<LineRenderer>();
        lineRenderer2.positionCount = 2;
        lineRenderer2.startWidth = 0.1f;
        lineRenderer2.endWidth = 0.1f;
        lineRenderer2.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer2.enabled = false;
        lineRenderer2.startColor = Color.red;
        lineRenderer2.endColor = Color.red;
        lineRenderer2.sortingOrder = 10;
    }

    new private void Awake()
    {
        base.Awake();
        originalSpeed = speed;
    }

    new private void Update()
    {
        if (_attackAnimTimer == 0)
        {
            base.Update();
        }
        Attack();
    }

    private void FixedUpdate()
    {
        _attackTimer = Mathf.Max(0, _attackTimer - 1);
    }

    public void Attack()
    {
        if (_attackTimer > 0) return;

        if (Vector3.Distance(transform.position, PlayerStats.Instance.playerTransform) > _attackRange)
        {
            return;
        }
        _attackTimer = AttackCD;

        Coroutine = StartCoroutine(ChargeAttack());
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D (collision);
        var damagable = collision.gameObject.GetComponent<Damagable>();
        if (damagable != null && damagable.IsPlayer)
        {
            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);
                lineRenderer.enabled = false;
                speed = originalSpeed;
            }
        }
    }

    private IEnumerator ChargeAttack()
    {
        _rb.linearVelocity = Vector2.zero;

        speed = 0;

        Vector3 playerTransform = PlayerStats.Instance.playerTransform;
        Vector3 directionToPlayer = (playerTransform - transform.position);
        // The two diagonal axes (a rotated coordinate system)
        Vector2 axis1 = new Vector2(1, 1).normalized;   // Up-Right
        Vector2 axis2 = new Vector2(-1, 1).normalized;  // Up-Left

        // Project the original vector onto each diagonal axis using the dot product
        float projection1 = Vector2.Dot(directionToPlayer, axis1);
        float projection2 = Vector2.Dot(directionToPlayer, axis2);

        // These are your two decomposed diagonal vectors
        Vector2 diagonalComponent1 = projection1 * axis1;
        Vector2 diagonalComponent2 = projection2 * axis2;

        // 50% to switch diagonalComponent1 and diagonalComponent2:
        if (Random.value < 0.5f)
        {
            // Swap the diagonal components
            var temp = diagonalComponent1;
            diagonalComponent1 = diagonalComponent2;
            diagonalComponent2 = temp;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + (Vector3)diagonalComponent1);
        lineRenderer.enabled = true;

        lineRenderer2.SetPosition(0, transform.position + (Vector3)diagonalComponent1);
        lineRenderer2.SetPosition(1, transform.position + (Vector3)diagonalComponent1 + (Vector3)diagonalComponent2);
        lineRenderer2.enabled = true;

        // Calculate hook pattern: first force is diagonal, second force is toward player
        for (int i = 0; i < 10; i++)
        {
            while (Time.timeScale == 0f)
                yield return null;
            yield return new WaitForFixedUpdate();
        }
        lineRenderer.enabled = false;
        lineRenderer2.enabled = false;

        // First force: diagonal
        _rb.AddForce(diagonalComponent1 * 200);
        animator.SetFloat("MoveX", diagonalComponent1.x);
        animator.SetFloat("MoveY", diagonalComponent1.y);

        for (int i = 0; i < 15; i++)
        {
            while (Time.timeScale == 0f)
                yield return null;
            yield return new WaitForFixedUpdate();
        }

        // Second force: direct to player
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(diagonalComponent2 * 400);
        animator.SetFloat("MoveX", diagonalComponent2.x);
        animator.SetFloat("MoveY", diagonalComponent2.y);

        speed = originalSpeed;
    }
}