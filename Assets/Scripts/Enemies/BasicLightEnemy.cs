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

        hp = 100f;
        flinchThreshold = hp / 2;

        attacks = new Attack[] {new Attack(1f, transform, 1.5f, 0, 5f, true, true),
                                new Attack(2f, transform, 1.5f, 0, 20f, false, true)};
        damageMult = new float[] { 1f, 0.5f, 1.5f };
    }
}
