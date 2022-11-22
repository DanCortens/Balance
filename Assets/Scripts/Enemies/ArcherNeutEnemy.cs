using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherNeutEnemy : EnemyAI
{
    void Start()
    {
        base.CustomStart();
    }

    override protected void EnemySpecificStart()
    {
        flying = false;
        grounded = false;
        chaser = false;
        hasFacing = true;
        flinching = false;

        hp = 100f;
        flinchThreshold = hp / 2;

        meleeAttacks = new Attack[] { };
        damageMult = new float[] { 1f, 1f, 1f };
    }
}
