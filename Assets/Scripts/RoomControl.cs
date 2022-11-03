using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControl : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public int[] types;
    public Transform roomPos;
    public Vector2 dimensions;
    public List<RoomControl> neighbours;
    public bool enemies;
    private CameraCont mainCam;
    private bool _filled;
    public bool filled
    {
        get { return _filled; } set { _filled = value; }
    }

    private EnemyPuppeteer puppeteer;

    void Start()
    {
        puppeteer = GameObject.Find("EnemyPuppeteer").GetComponent<EnemyPuppeteer>();
        mainCam = GameObject.Find("MainCamera").GetComponent<CameraCont>();
        filled = true;
    }

    public void PlayerEnters()
    {
        foreach (RoomControl n in neighbours)
            if (n.enemies && !n.filled)
                n.filled = puppeteer.FillRoom(n.spawnPoints, types, roomPos);
    }
    public void PlayerLeaves()
    {
        foreach (Transform child in roomPos.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        filled = false;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        mainCam.bounds = dimensions;
        mainCam.centre = transform.position;
        if (collision.gameObject.name == "player")
        {
            PlayerEnters();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            PlayerLeaves();
        }
    }
}
