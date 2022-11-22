using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNeutEnemy : EnemyAI
{
    void Start()
    {
        base.CustomStart();
    }
/*
    void Update()
   {
       if (this == null)
       FindObjectOfType<AudioManager>().Play("humandeath");
    } */

    
    override protected void EnemySpecificStart()
    {
        flying = false;
        grounded = false;
        chaser = true;
        turret = false;
        hasFacing = true;
        flinching = false;

        hp = 100f;
        flinchThreshold = hp / 2;

        meleeAttacks = new Attack[] { 
            new Attack(1f, new Vector2(0.5f, 0f), 1.5f, 0, 10f, true, 0.25f, "attack1"),
            new Attack(1f, new Vector2(0.5f, 0f), 1.5f, 0, 20f, false, 0.5f, "attack2")};
        damageMult = new float[] { 1f, 1f, 1f };
    }



}
