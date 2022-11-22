using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawbridge : MonoBehaviour
{
    private bool closed;
    public string soundName;
    public GameObject bridge;
    public LineRenderer line0;
    public LineRenderer line1;
    public Transform[] points;
    public float openPos;

    private void Start()
    {

        if (PlayerPrefs.HasKey(gameObject.name))
        {
            closed = (PlayerPrefs.GetInt(gameObject.name) == 0) ? false : true;
        }
        else
        {
            closed = true;
            PlayerPrefs.SetInt(gameObject.name, 1);
        }
        if (!closed)
            bridge.transform.localPosition = new Vector3(bridge.transform.localPosition.x, openPos);
        line0.SetPosition(0, points[0].position);
        line0.SetPosition(1, points[1].position);
        line1.SetPosition(0, points[2].position);
        line1.SetPosition(1, points[3].position);
    }

    public bool IsClosed()
    {
        return closed;
    }
    public void Open()
    {
        closed = false;
        FindObjectOfType<AudioManager>().Play(soundName);
        InvokeRepeating("Opening", 0f, 0.05f); 
    }

    private void Opening()
    {
        if (bridge.transform.localPosition.y > openPos)
        {
            bridge.transform.localPosition -= new Vector3(0f, 0.05f, 0f);
            line0.SetPosition(0, points[0].position);
            line0.SetPosition(1, points[1].position);
            line1.SetPosition(0, points[2].position);
            line1.SetPosition(1, points[3].position);
        }
            
        else
            CancelInvoke("Opening");
    }

    public void Save()
    {
        PlayerPrefs.SetInt(gameObject.name, (closed) ? 0 : 1);
    }
}
