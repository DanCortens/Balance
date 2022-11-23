using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    private Animator anim;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        anim = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        Vector3 newScale = player.transform.localScale;
        transform.localScale = newScale;
    }
    public void FadeIn(bool canInteract)
    {
        anim.SetBool("canInteract", canInteract);
    }
    public void FadeOut()
    {
        anim.SetBool("canInteract", false);
    }
}
