using System;
using System.Collections;
using TMPro;
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

        Vector3 targetPosition = transform.position;
        targetPosition.z = _mainCamera.transform.position.z; // Keep original camera z
        _mainCamera.transform.position = Vector3.Lerp(
            _mainCamera.transform.position,
            targetPosition,
            0.1f // Smooth factor, adjust as needed
        );

        // Rotate sprite based on Facing direction
        if (Facing != Vector2.zero)
        {
            float angle = Mathf.Atan2(Facing.y, Facing.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
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
        float multiplier = PlayerStats.Instance.GetCurrentDaddyMultiplier();

        Vector2 desiredVelocity = MoveInput * Speed * multiplier;
        Vector2 currentVelocity = Rigidbody.linearVelocity;
        Vector2 velocityChange = desiredVelocity - currentVelocity;
        Rigidbody.AddForce(velocityChange, ForceMode2D.Force);
    }


    public override void OnDeath()
    {
        ParticleSystem vfx = Instantiate(PlayerStats.Instance.deathvfx, transform.position, Quaternion.identity);
        vfx.transform.localScale = transform.localScale / 2f;
        vfx.Play();

        Debug.Log("Player Died!");
        GetComponent<Collider2D>().enabled = false;
        Time.timeScale = 0.5f;
        GameObject.Find("GameOverCanvas").GetComponent<GameOverCanvas>().Show();
    }

    public void OnRespawn()
    {
        GetComponent<Collider2D>().enabled = true;
        Time.timeScale = 1f;
        PlayerStats.Instance.limbs -= 1;
        PlayerStats.Instance.RepairLimb();
        PlayerStats.Instance.player.Heal(1);
        PlayerStats.Instance.player.transform.position = PlayerStats.Instance.bigDaddy.transform.position;
    }
}
