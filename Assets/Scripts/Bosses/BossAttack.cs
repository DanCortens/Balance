using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float windUp;        //time between start and attack
    public BossMove movement;   //begins immediately
    public bool hasAttack;      
    //if has attack, all the following must be set for melee
    public Vector2 attackOffset;//where in relation to the boss the attack will hit
    public float radius;
    public float damage;
    public int type;
    public bool rangedAttack;
    public GameObject projectile;
    public void StartAttack(Vector3 position)
    {
        movement.StartMove(position);
    }
    public float TotalTime()
    {
        return movement.duration;
    }
}