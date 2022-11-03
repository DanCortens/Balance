using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    
    AudioListener myAudioListener;
    public AudioMixer audioMixer;

    public static bool GameIsPaused = false;
    public static bool GameIsMenu = false;
    public static bool exitGame = false;

    public GameObject pauseMenuUI;
    public GameObject settingMenuUI;
    public GameObject exitGameUI;

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
        AudioListener.volume = 1;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        AudioListener.volume = 0f;
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
      public void QuitPrompt()
    {
    pauseMenuUI.SetActive(false);
    exitGameUI.SetActive(true);
    exitGame = true;
    }

    public void BackGame()
    {
        settingMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        GameIsMenu = false;
    }

    public void setVolume(float volume) 
    {
    audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

     public void ContinueGame()
    {
    pauseMenuUI.SetActive(true);
    exitGameUI.SetActive(false);
    exitGame = false;
    }

    public void QuitToMain()
    {
        AudioListener.volume = 1;
        Time.timeScale = 1f;
        SceneManager.LoadScene("HomeScene");
    }


}
