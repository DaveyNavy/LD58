using System;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour 
{
    public static PlayerStats Instance;
    public TMP_Text fleshText;
    public TMP_Text nextLimbCost;


    public int flesh = 100;
    public int limbs = 1;
    public Vector3 playerTransform;
    public PlayerMovement player;

    public int limbHealth = 100;

    public int DaddyFlesh = 75;
    public TextMeshProUGUI daddyStatusText;

    public ParticleSystem hitvfx;
    public ParticleSystem deathvfx;

    public BigDaddy bigDaddy;

    public bool IsGameOver;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        player = GetComponent<PlayerMovement>();
    }

    private float daddyFleshDecayTimer = 0f;

    private void Update()
    {
        playerTransform = transform.position;

        // Daddy flesh decay (1 flesh every second)
        daddyFleshDecayTimer += Time.deltaTime;
        if (daddyFleshDecayTimer >= 5f)
        {
            DaddyFlesh = Math.Max(0, DaddyFlesh - 1);
            daddyFleshDecayTimer = 0f;

            // Daddy buffs:
            if (DaddyFlesh < 10)
            {
                player.TakeDamage(1);
            }
        }
    }

    public float GetCurrentDaddyMultiplier()
    {
        if (DaddyFlesh >= 150) // 150+
        {
            return 1.5f;
        }
        else if (DaddyFlesh >= 100) // 100 -> 150
        {
            return 1.2f;
        }
        else if (DaddyFlesh >= 50) // 50 -> 100
        {
            return 1.0f;
        }
        else if (DaddyFlesh >= 10) // 10 -> 50
        {
            return 0.7f;
        }
        else // 0 -> 10
        {
            return 0.4f;
        }
    }

    public int GetCurrentLimbCost()
    {
        switch (limbs)
        {
            case 1:
                return 5;
            case 2:
                return 10;
            case 3:
                return 25;
            default:
                return -1;
        } 
    }

    public void UpdateFlesh(int change)
    {
        flesh += change;
        fleshText.text = "x" + flesh;
    }

    public void limbDecay(int change)
    {
        limbHealth -= change;
        if (limbHealth <= 0)
        {
            limbHealth = 100;
            if (limbs > 1)
            {
                limbs--;
                nextLimbCost.text = "Flesh for next limb: " + GetCurrentLimbCost();
            }
        }
    }

    public void AddLimb()
    {
        if (limbs < 4 && flesh >= GetCurrentLimbCost())
        {
            UpdateFlesh(-GetCurrentLimbCost());
            limbs++;
            player.Heal(100);
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.upgrade1, 0.7f);
            nextLimbCost.text = "Flesh for next limb: " + GetCurrentLimbCost();
        }
    }

    public void RepairLimb()
    {
        if (flesh > 0 && limbHealth < 100)
        {
            UpdateFlesh(-1);
            limbHealth = Mathf.Min(limbHealth + 5, 100);
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.chargeup1, 0.7f);
            player.Heal(3);
        }
    }
}