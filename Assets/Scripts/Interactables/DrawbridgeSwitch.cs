using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawbridgeSwitch : Interactable
{
    //switch for permanently unlocking a door
    public GameObject connectedDoor;
    private Animator anim;
    public LineRenderer line;
    public Transform endpoint;
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        line.SetPosition(1, transform.position);
        line.SetPosition(0, endpoint.position);
    }

    public override void Interact()
    {
        if (connectedDoor.GetComponent<Drawbridge>().IsClosed())
        {
            anim.SetTrigger("interact");
            connectedDoor.GetComponent<Drawbridge>().Open();
        }
            
    }
}
