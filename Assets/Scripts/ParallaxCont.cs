using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCont : MonoBehaviour
{
    private Camera cam;
    private Vector2 startPos;
    public float parallaxFactorX;
    public float parralaxFactorY;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(cam.transform.position.x * parallaxFactorX, cam.transform.position.y * parralaxFactorY);
        transform.position = startPos + offset;
    }
}
