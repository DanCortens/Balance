using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternPedestal : Interactable
{
    public CinematicController cc;
    public GameObject obstacle;
    public GameObject soulsReleasedEffect;
    public Animator medalAnim;
    public override void Interact()
    {
        PlayerStats.hasLantern = true;
        Vector2 offset = new Vector2(2f, 0f);
        cc.StartCinematic(new Vector2[] { (Vector2)transform.position,
                                        ((Vector2)transform.position + offset),
                                        ((Vector2)transform.position - offset),
                                        ((Vector2)transform.position + offset),
                                        ((Vector2)transform.position - offset),
                                        (Vector2)transform.position },
                            new float[] { 2f, 0.25f, 0.25f, 0.25f, 0.25f, 0.25f });
        FindObjectOfType<HudController>().ActivateBalanceBar();
        StartCoroutine(Crumble());
    }

    // Start is called before the first frame update
    void Start()
    {
        cc = GameObject.FindObjectOfType<CinematicController>();
    }
    private IEnumerator Crumble()
    {
        yield return new WaitForSeconds(1f);
        medalAnim.SetTrigger("crack");
        yield return new WaitForSeconds(0.5f);
        GameObject effect = Instantiate(soulsReleasedEffect, transform.position, Quaternion.identity);
        Destroy(transform.Find("medallion").gameObject);
        yield return new WaitForSeconds(4f);
        StartDestruction();
    }
    private void StartDestruction()
    {
        //play destruction animation
        obstacle.SetActive(false);
        //Calls audio
        FindObjectOfType<AudioManager>().Play("floorfall");
    }
}
