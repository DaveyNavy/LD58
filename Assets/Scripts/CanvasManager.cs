using UnityEngine;
using UnityEngine.SceneManagement;  

public class CanvasManager : MonoBehaviour
{
    [Header("References")]
    public GameObject pauseMenu; 

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("JoScene");
    }
}