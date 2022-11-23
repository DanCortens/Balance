using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public int id;
    public Vector3 startPos;
    public Vector3 openPos;
    public float toggleTime;
    public bool open;
    public LineRenderer line;
    public Vector3 lineOffset;
    private bool switching;
    private float slide;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(gameObject.name + id))
            open = (PlayerPrefs.GetInt(gameObject.name + id) == 0) ? false : true;
        startPos = transform.position;
        if (open)
            transform.position += openPos;
        switching = false;
        slide = 0f;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + openPos + lineOffset);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (switching)
        {
            slide += Time.deltaTime;
            if (slide < toggleTime)
            {
                if (open)
                    transform.position = Vector3.Lerp(startPos + openPos, startPos, slide / toggleTime);
                else
                    transform.position = Vector3.Lerp(startPos, startPos + openPos, slide / toggleTime);
            }
            else
            {
                open = !open;
                switching = false;
                transform.position = startPos;
                if (open)
                    transform.position += openPos;
            }
            line.SetPosition(0, transform.position);
        }
    }

    public void Toggle()
    {
        switching = true;
        slide = 0f;
    }
    public void Save()
    {
        PlayerPrefs.SetInt(gameObject.name + id, (open) ? 1 : 0);
    }
}
