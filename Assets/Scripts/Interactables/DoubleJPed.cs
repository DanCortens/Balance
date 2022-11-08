using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJPed : Interactable
{
    public GameObject doubleJ;

    public override void Interact()
    {
        if (doubleJ.activeInHierarchy)
        {
            PlayerStats.hasDoubleJ = true;
            doubleJ.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerStats.hasDoubleJ)
            doubleJ.SetActive(false);
    }
}
