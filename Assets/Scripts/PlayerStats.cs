using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour 
{
    public static PlayerStats Instance;

    public int flesh = 100;
    public int limbs = 1;
    public Vector3 playerTransform;
    public PlayerMovement player;

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
}
