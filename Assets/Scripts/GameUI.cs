using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    public Slider healthSlider;
    public Slider limbHealthSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSlider.maxValue = PlayerStats.Instance.player._maxHealth;
        limbHealthSlider.maxValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = PlayerStats.Instance.player._curHealth;
        limbHealthSlider.value = PlayerStats.Instance.limbHealth;
    }
}
