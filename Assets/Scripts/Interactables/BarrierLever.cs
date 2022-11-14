using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierLever : Interactable
{
    public Barrier[] connectedBarriers;
    public float toggleTime;
    [SerializeField] private bool toggling;
    private void Start()
    {
        toggling = false;
    }
    public override void Interact()
    {
        if (!toggling)
        {
            toggling = true;
            foreach (Barrier b in connectedBarriers)
                b.Toggle();
            StartCoroutine(ToggleTime());
        }
            
    }

    private IEnumerator ToggleTime()
    {
        yield return new WaitForSeconds(toggleTime);
        toggling = false;
    }
}
