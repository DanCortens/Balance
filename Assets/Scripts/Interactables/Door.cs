using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private BoxCollider2D obstacle;
    private bool locked;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        obstacle = transform.GetChild(0).GetComponent<BoxCollider2D>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("open", false);
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
        if (!anim.GetBool("open") && !locked)
        {
            anim.SetBool("open", true);
            obstacle.enabled = false;
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
        anim.SetBool("open", false);
        obstacle.enabled = true;
        Debug.Log("Closed");
        //play closed animation
        FindObjectOfType<AudioManager>().Play("door_Close");
    }

    
    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.gameObject.name == "player")
        {
            if (anim.GetBool("open"))
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
            
        }
        
    }
    public void Save()
    {
        PlayerPrefs.SetInt(gameObject.name, (locked)? 1 : 0);
    }
}
