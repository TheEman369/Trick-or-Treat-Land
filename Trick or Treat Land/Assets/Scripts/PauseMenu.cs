using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // only if you want MainMenu later

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;  // drag your Modal_PauseDim here in Inspector

    private bool isPaused = false;

    void Update()
    {
        // Listen for ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // resume game
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // freeze game
        isPaused = true;
    }

    // Hook these to your buttons
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // reset
        SceneManager.LoadScene("MainMenu"); // replace with your menu scene name
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
