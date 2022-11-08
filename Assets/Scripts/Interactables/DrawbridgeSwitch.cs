using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawbridgeSwitch : Interactable
{
    //switch for permanently unlocking a door
    public GameObject connectedDoor;

    public override void Interact()
    {
        connectedDoor.GetComponent<Drawbridge>().Open();
    }
}
