using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierLever : Interactable
{
    public Barrier[] connectedBarriers;
    public float toggleTime;
    [SerializeField] private bool toggling;
    private Animator anim;
    public LineRenderer line;
    public Transform endpoint;
    private void Start()
    {
        toggling = false;
        anim = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        line.SetPosition(1, transform.position);
        line.SetPosition(0, endpoint.position);
    }
    public override void Interact()
    {
        if (!toggling)
        {
            toggling = true;
            foreach (Barrier b in connectedBarriers)
                b.Toggle();
            anim.SetTrigger("interact");
            StartCoroutine(ToggleTime());
        }
            
    }

    private IEnumerator ToggleTime()
    {
        yield return new WaitForSeconds(toggleTime);
        toggling = false;
    }
}
