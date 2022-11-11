using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLightEnemy : EnemyAI
{
    override protected void EnemySpecificStart()
    {
        flying = false;
        grounded = false;
        chaser = false;
        hasFacing = true;
        turret = true;

        hp = 50f;
        flinchThreshold = hp / 2;

        meleeAttacks = new Attack[] {new Attack(1f, transform, 1.5f, 0, 5f, true, 0.5f)};
        damageMult = new float[] { 1f, 0.5f, 1.5f };
    }
}
