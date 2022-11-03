using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLoader : MonoBehaviour
{
    public TMP_Text loadingText;
    
    public void StartLoadScene(string sceneName)
    {
        StartCoroutine(LoadScene.Load(sceneName));
    }
    private void Update()
    {
        float prog = PlayerPrefs.GetFloat("progress", 0);
        loadingText.text = $"Loading: {prog}%";
    }

}
