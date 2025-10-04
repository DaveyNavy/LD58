using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Damagable
{
    public float Speed = 5f;
    private Vector2 _moveInput;
    [NonSerialized] public Rigidbody2D Rigidbody;
    private Camera _mainCamera;

    protected override void Awake()
    {
        base.Awake();
        //
        Rigidbody = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector2 desiredVelocity = _moveInput * Speed;
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
    }

    public override void OnDeath()
    {
        Debug.Log("Player Died!");
        Destroy(gameObject);
    }
}
