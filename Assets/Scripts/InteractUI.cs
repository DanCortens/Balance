using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
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
