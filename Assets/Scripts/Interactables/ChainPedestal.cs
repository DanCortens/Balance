using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainPedestal : Interactable
{
    public GameObject hook;

    public override void Interact()
    {
        if (hook.activeInHierarchy)
        {
            PlayerStats.hasChain = true;
            hook.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerStats.hasChain)
            hook.SetActive(false);
    }
}
