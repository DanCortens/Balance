using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPageScript : MonoBehaviour
{
    public static bool exitGame = false;
    public GameObject exitGameUI;

    public void GoToSettings()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("Level1");
    }
    // Scripts to navigate to either game scene or settings scene


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
if (Input.GetKeyDown(KeyCode.Escape))
        if (exitGame)
        {
        }
        else
        {
        exitGameUI.SetActive(true);
        exitGame = true;
        }
    }

    public void ContinueGame()
    {
    exitGameUI.SetActive(false);
    exitGame = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

}
