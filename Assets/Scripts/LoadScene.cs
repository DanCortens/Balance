using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadScene
{
    private static float _progress;
    public static float progress
    {
        get { return _progress; }
        set { if (_progress == value)
                return;
            _progress = value;
            UpdateProgress(_progress);
        }
    }
    public static IEnumerator Load(string scene)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
       
        while (!ao.isDone)
        {
            float prog = Mathf.Clamp01(ao.progress / 0.9f);
            progress = prog * 100f;
            yield return null;


        }
    }
    private static void UpdateProgress(float p)
    {
        PlayerPrefs.SetFloat("progress", p);
    }
}

