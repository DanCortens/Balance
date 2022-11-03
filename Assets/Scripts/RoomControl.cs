using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControl : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public Transform enemyHolder;
    public int[] types;
    public Vector2 dimensions;
    public bool enemies;
    private CameraCont mainCam;
    private bool _filled;
    public bool filled
    {
        get { return _filled; } set { _filled = value; }
    }
    public bool cleared = false;

    private EnemyPuppeteer puppeteer;

    void Start()
    {
        puppeteer = GameObject.Find("EnemyPuppeteer").GetComponent<EnemyPuppeteer>();
        mainCam = GameObject.Find("MainCamera").GetComponent<CameraCont>();
        filled = false;
    }

    public void PlayerEnters()
    {
        if (enemies && !filled)
        {
            filled = puppeteer.FillRoom(spawnPoints, types, enemyHolder.transform);
        }
        
        mainCam.centre = transform.position;
        mainCam.bounds = dimensions;
    }
    public void PlayerLeaves()
    {
        if (enemyHolder != null)
        {
            foreach (Transform child in enemyHolder.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            filled = false;
        }    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
