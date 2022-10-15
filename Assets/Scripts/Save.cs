using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


//Save object.
[System.Serializable]
public class Save
{
    //Currently only holds the player's save point ID and the scene name
    public string scene;
    public Transform savePoint;
    public string name;
    
}
