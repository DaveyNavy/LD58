using UnityEngine;

public class Enemy : Damagable
{
    private Renderer spriteRenderer;
    private Camera mainCamera;
    public int speed;

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

    void MoveTowardPlayer()
    {
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
