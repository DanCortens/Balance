using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDarkEnemy : EnemyAI
{
    void Start()
    {
        base.CustomStart();
    }
    void FixedUpdate()
    {
        CustomFixedUpdate();
    }
    override protected void EnemySpecificStart()
    {
        flying = false;
        grounded = false;
        chaser = true;
        hasFacing = true;
        flinching = false;

        hp = 100f;
        flinchThreshold = hp / 2;

        meleeAttacks = new Attack[] {};
        damageMult = new float[] { 1f, 0.5f, 1.5f };
    }
}
