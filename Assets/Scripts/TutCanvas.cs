using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutCanvas : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    [SerializeField] private Sprite sprite3;
    [SerializeField] private Sprite sprite4;
    [SerializeField] private Sprite sprite5;
    [SerializeField] private Sprite sprite6;
    [SerializeField] private Sprite sprite7;
    [SerializeField] private Sprite sprite8;
    [SerializeField] private Sprite spritetata;

    private int currentSpriteIndex = 0;
    private Sprite[] sprites;

    private void Start()
    {
        sprites = new Sprite[] { sprite1, sprite2, sprite3, sprite4, sprite5, sprite6, sprite7, sprite8 };
        if (sprites.Length > 0 && image != null)
        {
            image.sprite = sprites[0];
        }

        Time.timeScale = 0f; // Pause the game
        Time.fixedDeltaTime = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (sprites != null && sprites.Length > 0)
            {
                currentSpriteIndex = Mathf.Min(currentSpriteIndex + 1, sprites.Length - 1);
                image.sprite = sprites[currentSpriteIndex];
            }
            if (currentSpriteIndex == sprites.Length - 1)
            {
                gameObject.SetActive(false);
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
            }
        }
    }

    public void ShowOnDeath()
    {
        image.sprite = spritetata;
        gameObject.SetActive(true);
        currentSpriteIndex = sprites.Length - 1;
    }
}
