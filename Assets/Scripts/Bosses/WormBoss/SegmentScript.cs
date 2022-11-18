using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentScript : MonoBehaviour
{
    public Transform target;
    public float distance;
    private void Start()
    {
        //transform.LookAt(target);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float actualDist = Vector2.Distance(target.position, transform.position);
        if (actualDist > distance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, actualDist - distance);
            transform.up = target.position - transform.position;
        }
    }
}
