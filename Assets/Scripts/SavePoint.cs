using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SavePoint : Interactable
{
    public override void Interact()
    {
        GameManager gm = new GameManager();
        gm.CreateSave(gameObject.transform, SceneManager.GetActiveScene().name, "placeholder");
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
        }
    }
}
