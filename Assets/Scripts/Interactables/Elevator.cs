using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : Interactable
{
    public string sceneName;
    public override void Interact()
    {
        //play sound
        //play animation
        //load scene
        SceneManager.LoadScene(sceneName);
    }
}
