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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").GetComponent<Transform>();
        gse = gseObj.GetComponent<GameStateEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        //TO BE ADDED: rooms with bounds; if the player leaves the room, smooth follow to the new room
        //if (player.position.x > right || player.position.x < left || player.position.y > up || player.position < down) {}
        //else {}
        //follow the player 
        if (gse.state == GameStateEngine.State.Play)
            target.position = player.position;
        Vector3 targetPos = target.TransformPoint(new Vector3(0f,0f,-10f));
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, SMTH);
    }

    public void setTarget(Vector2 tar)
    {
        target.position = tar;
    }
}
