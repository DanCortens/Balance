using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager audioInstance;

    void Awake()
    {
        if (audioInstance == null)
        audioInstance = this;
        else {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start () 
    {
        Play("Start");
    }

    public void Play(string name)
    {
      Sound s = Array.Find(sounds, sound => sound.name == name);
      if (s == null)
      {
        Debug.LogWarning("Sound: " + name + "doesn't exist");
         return;
      }
        s.source.Play();
    }

        public void Pause(string name)
    {
      Sound s = Array.Find(sounds, sound => sound.name == name);
      if (s == null)
      {
        Debug.LogWarning("Sound: " + name + "doesn't exist");
         return;
      }
        s.source.Pause();
    }
}
