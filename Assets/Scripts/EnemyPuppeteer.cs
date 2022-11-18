using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPuppeteer : MonoBehaviour
{
    public List<GameObject> neutEnemyPrefabs;
    public List<GameObject> darkEnemyPrefabs;
    public List<GameObject> lightEnemyPrefabs;
    public event System.Action onWorldBalanceChanged;
    //public List<float> odds;
    private float _worldBalance;
    public float worldBalance
    {
        get { return _worldBalance; }
        set { 
            if (_worldBalance == value) return;
            _worldBalance = value;
            onWorldBalanceChanged?.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("worldBalance"))
            worldBalance = PlayerPrefs.GetFloat("worldBalance");
        else
            worldBalance = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool FillRoom(List<Transform> spawnPoints, int[] types, Transform enemyParent, RoomControl room)
    {
        bool swap = false;
        int i = 0;
        foreach (Transform sp in spawnPoints)
        {


            /*
             * first draft - randomly generated enemies; decided it would be better to have "slots" 
             * where each slot has a specific enemy general type (air, turret, basic, etc) and the 
             * light/dark/neut is set by filler
            float rand = Random.Range(0f, odds[odds.Count - 1]);
            int choice = 0; 
            foreach (float o in odds)
            {
                //if random choice is greater than odds, increase choice by one and check next
                if (o <= rand)
                    choice++;
                //break once the random choice is less than the next option
                else
                    break;
            }*/

            if (worldBalance <= -80f)
            {
                //only dark spawns
                SpawnEnemy(darkEnemyPrefabs[types[i]], sp.position, enemyParent, room);
            }
            else if (worldBalance <= -20f)
            {
                //mix of neutral and dark
                SpawnEnemy((swap)? neutEnemyPrefabs[types[i]] : darkEnemyPrefabs[types[i]]
                    , sp.position, enemyParent, room);
                swap = !swap;
            }
            else if (worldBalance < 20f)
            {
                //only neutral spawns
                SpawnEnemy(neutEnemyPrefabs[types[i]], sp.position, enemyParent, room);
            }
            else if (worldBalance < 80f)
            {
                //mix of light and neutral
                SpawnEnemy((swap) ? neutEnemyPrefabs[types[i]] : lightEnemyPrefabs[types[i]]
                    , sp.position, enemyParent, room);
                swap = !swap;
            }
            else
            {
                //only light spawns
                SpawnEnemy(lightEnemyPrefabs[types[i]], sp.position,enemyParent, room);
            }
            i++;
        }
        return true;
    }
    public void SpawnEnemy(GameObject spawned, Vector3 pos, Transform par, RoomControl room)
    {
        GameObject newSpawn = Instantiate(spawned, pos, Quaternion.identity, par);
        newSpawn.GetComponent<EnemyAI>().room = room;
        Debug.Log($"creating {newSpawn.name}");
    }
}
