using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLightEnemy : EnemyAI
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

        meleeAttacks = new Attack[] {new Attack(1f, transform, 1.5f, 1.1, 5f, true, 1f),
                                new Attack(1.5f, transform, 1.5f, 2, 20f, false, 1.5f)};
        damageMult = new float[] { 1f, 1.5f, 0.5f };
    }
}
