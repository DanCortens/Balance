using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public GameObject obstacle;
    private bool closed;
    private bool locked;

    // Start is called before the first frame update
    void Start()
    {
        closed = true;
        if (gameObject.name.Contains("locked"))
        {
            if (PlayerPrefs.HasKey(gameObject.name))
            {
                locked = (PlayerPrefs.GetInt(gameObject.name) == 0) ? false : true;
            }
            else
            {
                locked = true;
                PlayerPrefs.SetInt(gameObject.name, 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        if (closed && !locked)
        {
            closed = false;
            obstacle.SetActive(false);
            Debug.Log("Opened");
            //play open animation
            FindObjectOfType<AudioManager>().Play("door_Open");
        }
        else 
        {
            Close();
        }
    }

    private void Close()
    {
        closed = true;
        obstacle.SetActive(true);
        Debug.Log("Closed");
        //play closed animation
        FindObjectOfType<AudioManager>().Play("door_Close");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            //show interact prompt
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            //hide interact prompt
            if (!closed)
                Close();
        }
    }

    public void Unlock()
    {
        if (locked)
        {
            locked = false;
            //change sprite from locked to unlocked
            //play unlock sound
            FindObjectOfType<AudioManager>().Play("door_Unlock");
            PlayerPrefs.SetInt(gameObject.name, 0);
        }
        
    }
}
