using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            //show interact prompt
            FindObjectOfType<InteractUI>().FadeIn();
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "player")
        {
            FindObjectOfType<InteractUI>().FadeOut();
        }
    }
}
