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
    public GameObject GameOverCanvas;
    private Animator animator;
    private AudioSource heartbeatSource;
    private AudioSource footstepSource1;
    private AudioSource footstepSource2;

    protected override void Awake()
    {
        base.Awake();
        Rigidbody = GetComponent<Rigidbody2D>();
        Attack = GetComponent<PlayerAttack>();
        _mainCamera = Camera.main;
        animator = GetComponent<Animator>();

        heartbeatSource = SoundManager.PlayOnAudioSource(transform, SoundManager.Instance.heartbeat1);
        heartbeatSource.loop = true;
        heartbeatSource.volume = 0.3f;

        footstepSource1 = SoundManager.PlayOnAudioSource(transform, SoundManager.Instance.walk1, false);
        footstepSource1.loop = true;
        footstepSource1.volume = 0.3f;

        footstepSource2 = SoundManager.PlayOnAudioSource(transform, SoundManager.Instance.walk2, false);
        footstepSource2.loop = true;
        footstepSource2.volume = 0.3f;
    }

    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();

        if (MoveInput != Vector2.zero)
        {
            Facing = MoveInput.normalized;
            animator.SetFloat("MoveX", Facing.x);
            animator.SetFloat("MoveY", Facing.y);
            footstepSource1.Play();
            footstepSource2.Play();
        }
        else
        {
            footstepSource1.Stop();
            footstepSource2.Stop();
        }
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

        // Update heartbeat sound based on health:
        float healthRatio = (float)_curHealth / _maxHealth;
        if (healthRatio >= 0.5f)
        {
            heartbeatSource.volume = 0.3f;
        }
        else
        {
            // Lerp from 0.3f to 1f as healthRatio goes from 0.5 to 0
            float t = 1f - (healthRatio / 0.4f); // t = 0 at 0.5, t = 1 at 0
            heartbeatSource.volume = Mathf.Lerp(0.5f, 1f, t);
            heartbeatSource.pitch = Mathf.Lerp(1f, 2f, t);
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
        GameOverCanvas.SetActive(true);
    }

    public void OnRespawn()
    {
        GameOverCanvas.SetActive(false);
        GetComponent<Collider2D>().enabled = true;
        Time.timeScale = 1f;
        PlayerStats.Instance.limbs -= 1;
        PlayerStats.Instance.RepairLimb();
        PlayerStats.Instance.player.Heal(1);
        PlayerStats.Instance.player.transform.position = PlayerStats.Instance.bigDaddy.transform.position + new Vector3(0, -5, 0);
    }
}
