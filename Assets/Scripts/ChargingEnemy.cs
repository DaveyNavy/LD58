using UnityEngine;
using System.Collections;

public class ChargingEnemy : Enemy
{
    private int _attackTimer;
    private int _attackRange = 10;
    private int _attackAnimTimer = 500;
    [SerializeField] private int AttackCD;

    private int originalSpeed;

    private Coroutine Coroutine;

    private LineRenderer lineRenderer;

    protected override void Start()
    {
        base.Start();
        // Create and configure LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Assign a material (required)
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.enabled = false;

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
            }
            speed = 0;
        }
    }

    private IEnumerator ChargeAttack()
    {
        _attackAnimTimer = 150;
        _rb.linearVelocity = Vector2.zero;

        speed = 0;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, PlayerStats.Instance.playerTransform);
        Vector3 directionToPlayer = (PlayerStats.Instance.playerTransform - transform.position).normalized;

        lineRenderer.enabled = true;
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        lineRenderer.enabled = false;

        speed = originalSpeed * 15;
        Vector2 desiredVelocity = directionToPlayer * speed;
        Vector2 currentVelocity = _rb.linearVelocity;
        Vector2 velocityChange = desiredVelocity - currentVelocity;

        for (int i = 0; i < 30; i++) {
            _rb.AddForce(velocityChange, ForceMode2D.Force);
            yield return new WaitForFixedUpdate();
        }

        speed = 0;
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        speed = originalSpeed;
    }
}