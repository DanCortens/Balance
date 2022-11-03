using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternPedestal : Interactable
{
    public CinematicController cc;
    public GameObject obstacle;
    public override void Interact()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        cc = GameObject.FindObjectOfType<CinematicController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            PlayerStats.hasLantern = true;
            Vector2 offset = new Vector2 (0.5f,0f);
            cc.StartCinematic(new Vector2[] { (Vector2)transform.position, 
                                              ((Vector2)transform.position + offset),
                                              ((Vector2)transform.position - offset),
                                              ((Vector2)transform.position + offset),
                                              ((Vector2)transform.position - offset),
                                              (Vector2)transform.position },
                                new float[] { 2f, 0.25f, 0.25f, 0.25f, 0.25f, 0.25f });
            StartCoroutine(Crumble());
        }
    }
    private IEnumerator Crumble()
    {
        yield return new WaitForSeconds(3.25f);
        StartDestruction();
    }
    private void StartDestruction()
    {
        //play destruction animation
        obstacle.SetActive(false);
    }
}
