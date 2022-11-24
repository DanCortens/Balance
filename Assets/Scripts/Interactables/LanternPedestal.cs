using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternPedestal : Interactable
{
    public CinematicController cc;
    public GameObject obstacle;
    public GameObject soulsReleasedEffect;
    public Animator medalAnim;
    public GameObject localLant;
    private GameObject playerLant;

    public override void Interact()
    {
        PlayerStats.hasLantern = true;
        localLant.SetActive(false);
        playerLant.SetActive(true);
        canInteract = false;
        FindObjectOfType<InteractUI>().FadeOut();
        Vector2 offset = new Vector2(2f, 0f);

        
        cc.StartCinematic(new Vector2[] { (Vector2)transform.position,
                                        ((Vector2)transform.position + offset),
                                        ((Vector2)transform.position - offset),
                                        ((Vector2)transform.position + offset),
                                        ((Vector2)transform.position - offset),
                                        ((Vector2)transform.position + offset),
                                        ((Vector2)transform.position - offset),
                                        ((Vector2)transform.position + offset),
                                        ((Vector2)transform.position - offset),
                                        (Vector2)transform.position },
                            new float[] { 1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 4.25f });
        FindObjectOfType<HudController>().ActivateBalanceBar();
        StartCoroutine(Crumble());
    }

    // Start is called before the first frame update
    void Start()
    {
        playerLant = GameObject.Find("playerLantern");
        playerLant.SetActive(false);
        cc = GameObject.FindObjectOfType<CinematicController>();
    }
    private IEnumerator Crumble()
    {
        yield return new WaitForSeconds(1f);
        medalAnim.SetTrigger("crack");
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.Find("medallion").gameObject);
        GameObject effect = Instantiate(soulsReleasedEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        FindObjectOfType<AudioManager>().Play("banshee");
        
        yield return new WaitForSeconds(4f);
        //Calls audio
        FindObjectOfType<AudioManager>().Play("floorfall");
        yield return new WaitForSeconds(0.1f);
        StartDestruction();
    }
    private void StartDestruction()
    {

        obstacle.GetComponent<Animator>().SetTrigger("destroy");
        //play destruction animation
        obstacle.GetComponent<BoxCollider2D>().enabled = false;
        
    }
}
