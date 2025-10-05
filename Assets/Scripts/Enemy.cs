using UnityEngine;

public class Enemy : Damagable
{
    private Renderer spriteRenderer;
    private Camera mainCamera;
    private Animator animator;
    public int speed;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<Renderer>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (IsSpriteOnScreen())
        {
            MoveTowardPlayer();
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
        Vector2 direction = (PlayerStats.Instance.playerTransform - transform.position).normalized;
        Vector2 desiredVelocity = direction * speed;
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
