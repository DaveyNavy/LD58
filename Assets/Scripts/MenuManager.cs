using UnityEngine;
using UnityEngine.SceneManagement;  // for scene loading

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    public GameObject creditsPanel;  // assign in Inspector

    public void PlayGame()
    {
        // Replace "MainScene" with your gameplay scene name
        SceneManager.LoadScene("TileScene");
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game"); // works only in build
        Application.Quit();
    }
}