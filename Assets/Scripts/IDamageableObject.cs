using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageableObject
{
    void TakeDamage(float damage, int type, Vector2 enemyDir);
}
