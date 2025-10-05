using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.maxValue = PlayerStats.Instance.player._maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = PlayerStats.Instance.player._curHealth;
    }
}
