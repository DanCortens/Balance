using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCont : MonoBehaviour
{
    public Transform target;
    private Transform player;
    public GameObject gseObj;
    public RoomControl firstRoom;
    private GameStateEngine gse;
    private const float SMTH = 0.3f;
    private Vector3 vel = Vector3.zero;
    private Vector2 _bounds;
    private Vector3 shakeOffset;

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
        shakeOffset = new Vector3(0f, 0f, 0f);
        if (firstRoom is null)
        {
            bounds = new Vector2(25f, 15f);
            centre = transform.position;
        }
        else
        {
            bounds = firstRoom.dimensions;
            centre = firstRoom.transform.position;
        }
        

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
            float camWidth = Camera.main.orthographicSize * Camera.main.aspect;
            float leftBounds = _centre.x - (_bounds.x / 2) + camWidth;
            float rightBounds = _centre.x + (_bounds.x / 2) - camWidth;
            float topBounds = _centre.y - (_bounds.y / 2) + Camera.main.orthographicSize;
            float bottomBounds = _centre.y + (_bounds.y / 2) - Camera.main.orthographicSize;
            Vector3 clampTarget = new Vector3
                (Mathf.Clamp(targetPos.x, leftBounds, rightBounds),
                 Mathf.Clamp(targetPos.y, topBounds, bottomBounds),
                 targetPos.z);
            transform.position = Vector3.SmoothDamp(transform.position + shakeOffset, clampTarget, ref vel, SMTH);
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
    public void StartShake(Vector3 dir, float mag)
    {
        StartCoroutine(CamShake(dir, mag));
    }
    private IEnumerator CamShake(Vector3 dir, float mag)
    {
        shakeOffset = dir.normalized * mag;
        yield return new WaitForSeconds(0.1f);
        shakeOffset = new Vector3(0f, 0f, 0f);
    }

}
