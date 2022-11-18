using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public Transform[] segments;
    public LineRenderer lineRenderer;
    private void Start()
    {
        lineRenderer.positionCount = segments.Length;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
            lineRenderer.SetPosition(i, segments[i].position);
            
    }
}
