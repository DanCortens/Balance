using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackChain : MonoBehaviour
{
    public BossAttack[] bossAttacks;
    public int currentAttack;
    public string animBoolName;
    public bool counterable;
    public float cooldown;

    public bool UpdateCurrAttack()
    {
        currentAttack++;
        if (currentAttack < bossAttacks.Length)
            return false;
        
        else
        {
            currentAttack = 0;
            return true;
        }
            
    }
    public BossAttack GetCurrent()
    {
        return bossAttacks[currentAttack];
    }
}
