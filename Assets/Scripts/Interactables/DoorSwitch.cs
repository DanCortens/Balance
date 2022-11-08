using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : Interactable
{
    //switch for permanently unlocking a door
    public GameObject connectedDoor;

    public override void Interact()
    {
        connectedDoor.GetComponent<Door>().Unlock();
    }
}
