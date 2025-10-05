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
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        player = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        playerTransform = transform.position;
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
}