using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Damagable
{
    public float Speed = 5f;
    public Vector2 MoveInput;
    [NonSerialized] public Rigidbody2D Rigidbody;
    private Camera _mainCamera;
    public Vector2 Facing = Vector2.right;
    public PlayerAttack Attack;

    protected override void Awake()
    {
        base.Awake();
        //
        Rigidbody = GetComponent<Rigidbody2D>();
        Attack = GetComponent<PlayerAttack>();
        _mainCamera = Camera.main;
    }

    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
        if (MoveInput != Vector2.zero)
            Facing = MoveInput.normalized;
    }

    private void FixedUpdate()
    {
        float multiplier = PlayerStats.Instance.GetCurrentDaddyMultiplier();

        Vector2 desiredVelocity = MoveInput * Speed * multiplier;
        Vector2 currentVelocity = Rigidbody.linearVelocity;
        Vector2 velocityChange = desiredVelocity - currentVelocity;
        Rigidbody.AddForce(velocityChange, ForceMode2D.Force);
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.z = _mainCamera.transform.position.z; // Keep original camera z
        _mainCamera.transform.position = Vector3.Lerp(
            _mainCamera.transform.position,
            targetPosition,
            0.05f // Smooth factor, adjust as needed
        );

        // Rotate sprite based on Facing direction
        if (Facing != Vector2.zero)
        {
            float angle = Mathf.Atan2(Facing.y, Facing.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        // Clamp camera within bounds
    }

    public override void OnDeath()
    {
        Debug.Log("Player Died!");
        Destroy(gameObject);
    }
}
