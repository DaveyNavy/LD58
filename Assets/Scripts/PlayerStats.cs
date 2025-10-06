using System;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour 
{
    public static PlayerStats Instance;
    public TMP_Text fleshText;

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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        player = GetComponent<PlayerMovement>();

        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            daddyStatusText = GameObject.Find("Canvas").transform.Find("GameUI").transform.Find("DaddyStatusText").GetComponent<TextMeshProUGUI>();
        }
    }

    private float daddyFleshDecayTimer = 0f;

    private void Update()
    {
        playerTransform = transform.position;

        // Daddy flesh decay (1 flesh every second)
        daddyFleshDecayTimer += Time.deltaTime;
        if (daddyFleshDecayTimer >= 2f)
        {
            DaddyFlesh = Math.Max(0, DaddyFlesh - 1);
            daddyFleshDecayTimer = 0f;

            // Daddy buffs:
            if (DaddyFlesh < 10)
            {
                player.TakeDamage(1);
            }
        }

        if (daddyStatusText == null) return;

        if (DaddyFlesh >= 150) // 150+
        {
            daddyStatusText.text = "Big Daddy is gonna bust!";
        }
        else if (DaddyFlesh >= 100) // 100 -> 150
        {
            daddyStatusText.text = "Big Daddy is Happy!";
        }
        else if (DaddyFlesh >= 50) // 50 -> 100
        {
            daddyStatusText.text = "Big Daddy is Content!";
        }
        else if (DaddyFlesh > 10) // 10 -> 50
        {
            daddyStatusText.text = "Big Daddy is Sad! (" + (50 - DaddyFlesh) + " more flesh needed)";
        }
        else // 0 -> 10
        {
            daddyStatusText.text = "Big Daddy is grief-stricken! (" + (50 - DaddyFlesh) + " more flesh needed)";
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
                return 25;
            case 2:
                return 50;
            case 3:
                return 100;
            default:
                return -1;
        } 
    }

    public void UpdateFlesh(int change)
    {
        flesh += change;
        fleshText.text = "Flesh: " + flesh;
    }

    public void limbDecay()
    {
        limbHealth--;
        if (limbHealth <= 0)
        {
            limbHealth = 100;
            limbs--;
            if (limbs <= 0)
            {
                Debug.Log("Player Died!");
            }
        }
    }

    public void AddLimb()
    {
        if (limbs < 4 && flesh >= GetCurrentLimbCost())
        {
            UpdateFlesh(-GetCurrentLimbCost());
            limbs++;
        }
    }

    public void RepairLimb()
    {
        if (flesh > 0 && limbHealth < 100)
        {
            UpdateFlesh(-1);
            limbHealth = Mathf.Min(limbHealth + 5, 100);
        }
    }
}