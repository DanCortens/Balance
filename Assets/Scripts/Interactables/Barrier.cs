using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public int id;
    public Vector3 closedPos;
    public Vector3 openPos;
    public float toggleTime;
    public bool open;
    private bool switching;
    private float slide;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(gameObject.name + id))
            open = (PlayerPrefs.GetInt(gameObject.name + id) == 0) ? false : true;
        
        transform.position = (open) ? openPos : closedPos;
        switching = false;
        slide = 0f;
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
                    transform.position = Vector3.Lerp(openPos, closedPos, slide / toggleTime);
                else
                    transform.position = Vector3.Lerp(closedPos, openPos, slide / toggleTime);
            }
            else
            {
                open = !open;
                switching = false;
                transform.position = (open) ? openPos : closedPos;
            }
            
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
