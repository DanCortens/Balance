using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCont : MonoBehaviour
{
    public Transform target;
    private Transform player;
    public GameObject gseObj;
    private GameStateEngine gse;
    private const float SMTH = 0.3f;
    private Vector3 vel = Vector3.zero;
    private Vector2 _bounds;
    public Vector2 bounds
    {
        set { _bounds = value; }
    }
    private Vector2 _centre;
    public Vector2 centre
    {
        set { _centre = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        //temp: testing
        bounds = new Vector2(5f,4.2f);
        centre = new Vector2(-3.25f,-1.5f);
        //temp temp temp


        player = GameObject.Find("player").GetComponent<Transform>();
        gse = gseObj.GetComponent<GameStateEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos;
        if (gse.state == GameStateEngine.State.Play)
        {
            target.position = player.position;
            targetPos = target.TransformPoint(new Vector3(0f, 0f, -10f));
            Vector3 clampTarget = new Vector3 
                (Mathf.Clamp(targetPos.x, _centre.x - (_bounds.x/2), _centre.x + (_bounds.x / 2)),
                 Mathf.Clamp(targetPos.y, _centre.y - (_bounds.y/2), _centre.y + (_bounds.y / 2)),
                 targetPos.z);
            transform.position = Vector3.SmoothDamp(transform.position, clampTarget, ref vel, SMTH);
        }
        else
        {
            targetPos = target.TransformPoint(new Vector3(0f, 0f, -10f));
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, SMTH);
        }
    }

    public void setTarget(Vector2 tar)
    {
        target.position = tar;
    }

}
