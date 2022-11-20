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

    void FixedUpdate()
    {
        CustomFixedUpdate();
    }
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

        meleeAttacks = new Attack[] {};
        damageMult = new float[] { 1f, 1f, 1f };
    }



}
