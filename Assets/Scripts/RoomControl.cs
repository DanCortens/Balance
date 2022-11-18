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
    public bool cleared;
    public int killCount;
    public int killsReq;
    public Door[] doors;

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
            filled = puppeteer.FillRoom(spawnPoints, types, enemyHolder.transform, this);
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
        if (!cleared)
            killCount = 0;
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

    public void EnemyKilled()
    {
        killCount++;
        if (killCount >= killsReq)
            Cleared();
    }
    public void Cleared()
    {
        cleared = true;
        foreach (Door d in doors)
            d.Unlock();

        FindObjectOfType<CinematicController>().StartCinematic(
            new Vector2[] { doors[0].transform.position }, new float[] { 2f });
    }
}
