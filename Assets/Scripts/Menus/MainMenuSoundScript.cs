using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSoundScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private static MainMenuSoundScript instance = null;
    public static MainMenuSoundScript Instance 
    {
        get {return instance;}
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        
    }
}
