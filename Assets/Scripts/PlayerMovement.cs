using System;
using System.Collections;
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


    public KeyCode key = KeyCode.LeftShift;
    public float holdTime = 1f;
    private float holdTimer = 0f;
    private bool actionTriggered = false;

    void Update()
    {
        if (Input.GetKey(key))
        {
            Speed = 0;
            holdTimer += Time.deltaTime;

            if (!actionTriggered && holdTimer >= holdTime)
            {
                actionTriggered = true;
                OnHoldComplete();
            }
        }
        else
        {
            holdTimer = 0f;
            actionTriggered = false;
            Speed = 10f;
        }
    }

    void OnHoldComplete()
    {
        if (PlayerStats.Instance.flesh > 0 && PlayerStats.Instance.limbHealth < 100)
        {
            PlayerStats.Instance.RepairLimb();
        }
        else if (PlayerStats.Instance.limbs < 4 && PlayerStats.Instance.flesh >= PlayerStats.Instance.GetCurrentLimbCost())
        {
            PlayerStats.Instance.AddLimb();
        }
        holdTimer = 0f;
        actionTriggered = false;
    }
    private void FixedUpdate()
    {
        Vector2 desiredVelocity = MoveInput * Speed;
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
