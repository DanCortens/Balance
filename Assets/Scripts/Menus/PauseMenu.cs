using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool GameIsMenu = false;

    public GameObject pauseMenuUI;
    public GameObject settingMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsMenu)
            {
            }
            else if (GameIsPaused) 
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
    pauseMenuUI.SetActive(false);
    settingMenuUI.SetActive(true);
    GameIsMenu = true;
    }
      public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void BackGame()
    {
        settingMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        GameIsMenu = false;
    }
}
