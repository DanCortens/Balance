using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainHinge : MonoBehaviour
{
    public PlayerController playerCont;
    public GameObject hingeObj;
    public Rigidbody2D hingeRb;
    public DistanceJoint2D hinge;
    public LineRenderer chainRender;
    private Vector2 hookPoint;
    private float dist;
    private List<Vector2> chainPos = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        hinge.enabled = false;
        chainRender.positionCount = 2;
    }

    public void Hook(Transform hp)
    {
        chainRender.enabled = true;
        hookPoint = hp.position;
        if (!chainPos.Contains(hookPoint))
        {
            chainPos.Add(hookPoint);
            
            hingeRb.transform.position = hookPoint;
            dist = Vector2.Distance(transform.position, hookPoint);
            hinge.enabled = true;
            hinge.distance = dist;
            chainRender.SetPosition(0, transform.position);
            chainRender.SetPosition(1, hookPoint);
        }
    }
    public void Unhook()
    {
        chainRender.enabled = false;
        chainRender.SetPosition(1, transform.position);
        chainPos.Clear();
        hinge.enabled = false;
    }

    public void UpdateChain()
    {
        chainRender.SetPosition(0, transform.position);
        hingeRb.transform.position = hookPoint;

    }
}
